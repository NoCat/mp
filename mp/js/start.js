var mp;
(function (mp) {
    var start;
    (function (_start) {
        $(function () {
            $(document).on('click', '.login-btn', function () {
                mp.modal.ShowLogin();
                return false;
            });
            $(document).on('click', '.signup-btn', function () {
                mp.modal.ShowSignup();
                return false;
            });
            $(document).on('click', '.resave-btn', function (e) {
                var btn = $(e.currentTarget);
                var id = btn.data('id');
                var url = '/image/' + id + '/resave';
                mp.modal.ShowImage(url, '转存', function () {
                    mp.modal.MessageBox('转存成功', '提示', function () {
                        mp.modal.Close();
                    });
                });
                return false;
            });
            $(document).on('click', '.image-edit-btn', function (e) {
                var btn = $(e.currentTarget);
                var id = btn.data('id');
                var url = '/image/' + id + '/edit';
                mp.modal.ShowImage(url, '编辑');
                return false;
            });
            $(document).on('click', '.praise-btn', function (e) {
                var btn = $(e.currentTarget);
                var id = btn.data('id');
                var url = '/image/' + id + '/praise';
                $.post(url, function (result) {
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
                        mp.modal.MessageBox(result.Message);
                    }
                }, 'json');
                return false;
            });
            $(document).on('click', '.cancel-praise-btn', function (e) {
                var btn = $(e.currentTarget);
                var id = btn.data('id');
                var url = '/image/' + id + '/cancelpraise';
                $.post(url, function (result) {
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
                        mp.modal.MessageBox(result.Message);
                    }
                }, 'json');
                return false;
            });
            $(document).on('change', '#upload', function (e) {
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
                };
                up.onDone = function (datas) {
                    var uploadDatas = [];
                    for (var i = 0; i < datas.length; i++) {
                        uploadDatas.push({ id: datas[i].Data.id, description: datas[i].File.name });
                    }
                    $("#image-modal").modal({ backdrop: 'static' });
                    $("#image-modal.modal-body").load("\image\Add\?id=" + datas[0].Data.id);
                };
            });
        });
        var Uploader = (function () {
            function Uploader(file) {
                this.onProgress = null;
                this.onDone = null;
                this.onError = null;
                this.url = '';
                this.chunkSize = 256 * 1024;
                this.chunks = 0;
                this.name = '' + new Date().getTime() + Math.random();
                this.chunk = 0;
                this.isStop = false;
                this.file = file;
                this.chunks = Math.ceil(this.file.size / this.chunkSize);
            }
            Uploader.prototype.start = function () {
                this.Upload();
            };
            Uploader.prototype.stop = function () {
                this.isStop = true;
            };
            Uploader.prototype.Upload = function () {
                var _this = this;
                if (this.isStop == true)
                    return;
                var xhr = new XMLHttpRequest();
                xhr.open("post", this.url);
                xhr.responseType = 'json';
                xhr.onload = function (ev) {
                    var result = xhr.response;
                    if (result.Success == false) {
                        _this.isStop = true;
                        if (_this.onError != null) {
                            _this.onError(result.Message);
                        }
                    }
                    else {
                        _this.chunk++;
                        if (_this.chunk == _this.chunks) {
                            if (_this.onDone != null) {
                                var data = {};
                                data.File = _this.file;
                                data.Data = result.Data;
                                _this.onDone(data);
                            }
                        }
                        else {
                            _this.Upload();
                        }
                    }
                };
                xhr.onerror = function (ev) {
                    _this.isStop = true;
                    if (_this.onError != null) {
                        _this.onError("请求出错！(" + xhr.status + ")");
                    }
                };
                xhr.upload.onprogress = function (ev) {
                    if (ev.lengthComputable == true) {
                        var loaded = ev.loaded + _this.chunk * _this.chunkSize;
                        if (_this.onProgress != null) {
                            _this.onProgress(loaded / _this.file.size);
                        }
                    }
                };
                var start = this.chunk * this.chunkSize;
                var end = start + this.chunkSize;
                var data = new FormData();
                data.append("chunk", this.chunk);
                data.append("name", this.name);
                data.append("chunks", this.chunks);
                data.append("data", this.file.slice(start, end));
                xhr.send(data);
            };
            return Uploader;
        })();
        var BatchUploader = (function () {
            function BatchUploader() {
                this.totalSize = 0;
                this.loadedSize = 0;
                this.currentIndex = 0;
                this.url = "";
                this.chunkSize = 256 * 1024;
                this.onProgress = null;
                this.onDone = null;
                this.onError = null;
                this.list = new Array();
                this.doneDataList = [];
            }
            BatchUploader.prototype.add = function (file) {
                var uploader = new Uploader(file);
                uploader.url = this.url;
                uploader.chunkSize = this.chunkSize;
                this.list.push(uploader);
                this.totalSize += file.size;
            };
            BatchUploader.prototype.start = function () {
                var _this = this;
                if (this.currentIndex != this.list.length) {
                    var u = this.list[this.currentIndex];
                    u.onError = function (msg) {
                        if (_this.onError != null)
                            _this.onError(msg);
                    };
                    u.onProgress = function (p) {
                        var p1 = (_this.loadedSize + u.file.size * p) / _this.totalSize;
                        if (_this.onProgress != null)
                            _this.onProgress(p1);
                    };
                    u.onDone = function (data) {
                        _this.doneDataList.push(data);
                        _this.loadedSize += u.file.size;
                        _this.start();
                    };
                    u.start();
                    this.currentIndex++;
                }
                else {
                    if (this.onDone != null)
                        this.onDone(this.doneDataList);
                }
            };
            BatchUploader.prototype.stop = function () {
                this.list[this.currentIndex].stop();
            };
            return BatchUploader;
        })();
        _start.BatchUploader = BatchUploader;
    })(start = mp.start || (mp.start = {}));
})(mp || (mp = {}));
