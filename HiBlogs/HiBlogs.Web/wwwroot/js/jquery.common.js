;
//整理js常用功能
(function ($, window) {
    //责任链   
    //Function.prototype.setNext = function (successor) {
    //    return this.successor = successor;
    //}
    //Function.prototype.executeNext = function () {
    //    this.successor && this.successor.apply(this, arguments);
    //}
    //扩展
    $.extend({
        //获取url参数
        queryString: function (name) {
            var search = window.location.search + '';
            if (search.charAt(0) != '?') {
                return undefined;
            }
            else {
                search = search.replace('?', '').split('&');
                for (var i = 0; i < search.length; i++) {
                    if (search[i].split('=')[0] == name) {
                        return decodeURI(search[i].split('=')[1]);
                    }
                }
                return undefined;
            }
        },
        //返回修改后的url
        setUrlParameter: function (key, value) {
            var urlsplit = location.href.split("?");
            if (urlsplit.length === 1) {
                return location.href + "?" + key + "=" + value;
            }
            var urlKeyValues = urlsplit[1].split("&");
            var urlright = [];
            var isUpdateKeyValue = false;
            urlKeyValues.forEach(function (valueStr) {
                var keyStr = valueStr.split("=")[0].trim();
                if (keyStr === key) {
                    urlright.push(key + "=" + value);
                    isUpdateKeyValue = true;
                }
                else {
                    urlright.push(valueStr);
                }
            });
            if (!isUpdateKeyValue) {
                urlright.push(key + "=" + value);
            }
            return urlsplit[0] + "?" + urlright.join("&");
        },
        //设置 或 获取 url参数值
        //$.urlHelper.parameter("page")
        //$.urlHelper.parameter("page", 1)
        parameter: function () {
            if (arguments.length === 1) {
                return this.queryString(arguments[0]);
            }
            else if (arguments.length === 2) {
                return this.setUrlParameter(arguments[0], arguments[1]);
            }
        },
        //塞入历史记录，并改变当前url
        pushState: function (url) {
            history.pushState(null, null, url);
        },
        //取num位小数点，不够补0
        formatNumber: function (value, num) {
            var a, b, c, i;
            a = value.toString();
            b = a.indexOf(".");
            c = a.length;
            if (num == 0) {
                if (b != -1) {
                    a = a.substring(0, b);
                }
            } else {//如果没有小数点
                if (b == -1) {
                    a = a + ".";
                    for (i = 1; i <= num; i++) {
                        a = a + "0";
                    }
                } else {//有小数点,超出位数自动截取,否则补0
                    a = a.substring(0, b + num + 1);
                    for (i = c; i <= b + num; i++) {
                        a = a + "0";
                    }
                }
            }
            return a;
        },
        //计数执行（可用户多个异步都完成后执行）
        deferCall: function () {
            var DeferCall = function () {
                this.number = 0;
                this.fn = function () { };
            };
            //添加计数
            DeferCall.prototype.add = function (number) {
                $.isNumeric(number) ? this.number += number : this.number++;
            }
            //执行回调
            DeferCall.prototype.callBack = function (fn) {
                this.fn = $.isFunction(fn) ? fn : this.fn;
            };
            //延迟执行
            DeferCall.prototype.execute = function (callBack, arguments) {
                this.number--;
                if (this.number === 0) {
                    this.fn();
                    $.isFunction(callBack) && callBack(arguments);
                }
            }
            return new DeferCall();
        },
        //单例转换
        getSingle: function (obj) {
            var instance;
            return function () {
                return instance || (instance = new obj());
            }
        }, 
        /*1.用浏览器内部转换器实现html转码*/
        htmlEncode: function (html) {
            //1.首先动态创建一个容器标签元素，如DIV
            var temp = document.createElement("div");
            //2.然后将要转换的字符串设置为这个元素的innerText(ie支持)或者textContent(火狐，google支持)
            (temp.textContent != undefined) ? (temp.textContent = html) : (temp.innerText = html);
            //3.最后返回这个元素的innerHTML，即得到经过HTML编码转换的字符串了
            var output = temp.innerHTML;
            temp = null;
            return output;
        }, 
        /*2.用浏览器内部转换器实现html解码*/
        htmlDecode: function (text) {
            //1.首先动态创建一个容器标签元素，如DIV
            var temp = document.createElement("div");
            //2.然后将要转换的字符串设置为这个元素的innerHTML(ie，火狐，google都支持)
            temp.innerHTML = text;
            //3.最后返回这个元素的innerText(ie支持)或者textContent(火狐，google支持)，即得到经过HTML解码的字符串了。
            var output = temp.innerText || temp.textContent;
            temp = null;
            return output; 
        } 
    });
})(jQuery, window);




