var mp;
(function (mp) {
    var uploader;
    (function (_uploader) {
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
        _uploader.Uploader = Uploader;
        var BatchUploader = (function () {
            function BatchUploader() {
                this.totalSize = 0;
                this.loadedSize = 0;
                this._stop = false;
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
                        var c = _this.currentIndex;
                        if (_this.onProgress != null)
                            _this.onProgress(p1, c);
                        if (_this._stop == true) {
                            u.stop();
                            return;
                        }
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
                this._stop = true;
            };
            return BatchUploader;
        })();
        _uploader.BatchUploader = BatchUploader;
    })(uploader = mp.uploader || (mp.uploader = {}));
})(mp || (mp = {}));
