﻿/// <reference path="mp.ts" />
/// <reference path="modal.ts" />
/// <reference path="uploader.ts" />

module mp.start {
    $(() => {
        $(document).on('click', '.login-btn',() => {
            modal.ShowLogin();
            return false;
        });

        $(document).on('click', '.signup-btn',() => {
            modal.ShowSignup();
            return false;
        });

        $(document).on('click', '.resave-btn',(e) => {
            var btn = $(e.currentTarget);
            var id = btn.data('id');

            var url = '/image/' + id + '/resave';
            modal.ShowImage(url, '转存',() => {
                modal.MessageBox('转存成功', '提示',() => { modal.Close(); });
            });

            return false;
        });

        $(document).on('click', '.image-edit-btn',(e) => {
            var btn = $(e.currentTarget);
            var id = btn.data('id');

            var url = '/image/' + id + '/edit';
            modal.ShowImage(url, '编辑');

            return false;
        });

        $(document).on('click', '.praise-btn',(e) => {
            var btn = $(e.currentTarget);
            var id = btn.data('id');

            var url = '/image/' + id + '/praise';
            $.post(url,(result: AjaxResult) => {
                if (result.Success) {
                    var count = result.Data.count;
                    var text = btn.find('.text');
                    if (count == 0)
                        text.text('');
                    else
                        text.text(count);

                    btn.removeClass('praise-btn');
                    btn.addClass('cancel-praise-btn');
                }
                else {
                    modal.MessageBox(result.Message);
                }
            }, 'json');

            return false;
        });

        $(document).on('click', '.cancel-praise-btn',(e) => {
            var btn = $(e.currentTarget);
            var id = btn.data('id');

            var url = '/image/' + id + '/cancelpraise';
            $.post(url,(result: AjaxResult) => {
                if (result.Success) {
                    var count = result.Data.count;
                    var text = btn.find('.text');
                    if (count == 0)
                        text.text('');
                    else
                        text.text(count);

                    btn.removeClass('cancel-praise-btn');
                    btn.addClass('praise-btn');
                }
                else {
                    modal.MessageBox(result.Message);
                }
            }, 'json');

            return false;
        });

        $(document).on('change', '#upload',(e) => {
            var files = $(e.currentTarget).prop('files');
            if (files.length == 0) {
                return false;
            }
            $("#modal").modal('show');
            var btn = $(e.currentTarget);
            var progress = $(".progress");
            var up = new BatchUploader();
            up.url = "/upload";
            for (var i = 0; i < files.length; i++) {
                up.add(files[i]);
            }

            up.onProgress = function (p) {
                p = Math.floor(p * 100);
                progress.css({ "width": p + "%" });
            }

            up.onDone = function (datas) {
                //alert(JSON.stringify(datas));

                var uploadDatas = [];
                for (var i = 0; i < datas.length; i++) {
                    uploadDatas.push({ id: datas[i].Data.id, description: datas[i].File.name });
                }

                $("#image-modal").modal({ backdrop: 'static' });

                $("#image-modal.modal-body").load("\image\Add\?id="+datas[0].Data.id);
            }
        })
    });
    interface UploaderDoneData {
        File?: File;
        Data?: any;
    }

     class Uploader {
        onProgress: (percentage: number) => void = null;
        onDone: (data: UploaderDoneData) => void = null;
        onError: (msg: string) => void = null;

        url = '';
        chunkSize = 256 * 1024;
        file: File;

        private chunks: number = 0;
        private name = '' + new Date().getTime() + Math.random();
        private chunk = 0;
        private isStop = false;

        constructor(file: File) {
            this.file = file;
            this.chunks = Math.ceil(this.file.size / this.chunkSize);
        }

        start(): void {
            this.Upload();
        }

        stop(): void {
            this.isStop = true;
        }

        private Upload() {
            if (this.isStop == true)
                return;

            //准备xhr对象
            var xhr = new XMLHttpRequest();
            xhr.open("post", this.url);
            xhr.responseType = 'json';
            xhr.onload = (ev) => {
                var result: AjaxResult = xhr.response;
                if (result.Success == false) {
                    this.isStop = true;
                    if (this.onError != null) {
                        this.onError(result.Message);
                    }
                }
                else {
                    this.chunk++;
                    if (this.chunk == this.chunks) {
                        if (this.onDone != null) {
                            var data: UploaderDoneData = {};
                            data.File = this.file;
                            data.Data = result.Data;
                            this.onDone(data);
                        }
                    }
                    else {
                        this.Upload();
                    }
                }
            };
            xhr.onerror = (ev) => {
                this.isStop = true;
                if (this.onError != null) {
                    this.onError("请求出错！(" + xhr.status + ")");
                }
            }
            xhr.upload.onprogress = (ev) => {
                if (ev.lengthComputable == true) {
                    var loaded = ev.loaded + this.chunk * this.chunkSize;
                    if (this.onProgress != null) {
                        this.onProgress(loaded / this.file.size);
                    }
                }
            }

            //准备要发送的数据
            var start = this.chunk * this.chunkSize;
            var end = start + this.chunkSize;

            var data = new FormData();
            data.append("chunk", this.chunk);
            data.append("name", this.name);
            data.append("chunks", this.chunks);
            data.append("data", this.file.slice(start, end));

            xhr.send(data);
        }
    }
    export class BatchUploader {
        private list: Array<Uploader>;
        private totalSize = 0;
        private loadedSize = 0;
        private doneDataList: Array<UploaderDoneData>;
        currentIndex = 0;
        url = "";
        chunkSize = 256 * 1024;

        onProgress: (percentage: number) => void = null;
        onDone: (datas: Array<UploaderDoneData>) => void = null;
        onError: (msg: string) => void = null;

        constructor() {
            this.list = new Array<Uploader>();
            this.doneDataList = [];
        }

        add(file: File) {
            var uploader = new Uploader(file);
            uploader.url = this.url;
            uploader.chunkSize = this.chunkSize;
            this.list.push(uploader);
            this.totalSize += file.size;
        }

        start() {
            if (this.currentIndex != this.list.length) {
                var u = this.list[this.currentIndex];
                u.onError = (msg) => {
                    if (this.onError != null)
                        this.onError(msg);
                };
                u.onProgress = (p) => {
                    var p1 = (this.loadedSize + u.file.size * p) / this.totalSize;
                    if (this.onProgress != null)
                        this.onProgress(p1);
                };
                u.onDone = (data) => {
                    this.doneDataList.push(data);
                    this.loadedSize += u.file.size;
                    this.start();
                };
                u.start();
                this.currentIndex++;
            }
            else {
                if (this.onDone != null)
                    this.onDone(this.doneDataList);
            }
        }
        stop() {
            this.list[this.currentIndex].stop();
        }
    }
}
