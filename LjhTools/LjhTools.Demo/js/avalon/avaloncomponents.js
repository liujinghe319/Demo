avalon.component('ms-select', {
    template: '<select ms-attr="{id:##cid,disabled:##disabled}" ms-css="##mattr" ms-class="##mclass" class="bmselect w150"><option value=""></option><option ms-for="item in ##data" ms-attr="{value:item.value}">{{item.label}}</option></select> ',
    defaults: {
        cid: "ms1",
        data: [],
        selected: 0,
        disabled:false,
        selmodel: {},
        title: "请选择",
        choose: avalon.noop,
        mclass: "",
        mattr: "",
        search: true,
        dropdownParent: "",
        onInit: function () {
            if (this.cid == "ms1") {
                this.cid += zutil.getguid();
            }

        },
        onReady: function () {
            var _this = this;


            var options = {
                placeholder: this.title,
                language: "zh-CN",
                allowClear: true 
            };
            if (!_this.search) {
                options.minimumResultsForSearch = Infinity;
            }
            if (this.dropdownParent != "") {
                options.dropdownParent = $(this.dropdownParent);
            }
            var $select = $("#" + this.cid); 
            $select.select2(options);
            $select.on("change", function (e) { 
                _this.selmodel.value = $select.val();
                _this.selmodel.label = $select.find("option:selected").text(); 
                _this.choose(_this.selmodel.value, _this.selmodel.label);
            });

            this.$watch("selmodel", function observe(a, b) {
                if (a && a.value != b.value) { 
                    $select.val(a.value).trigger('change'); 
                }
            });

            this.$watch("data", function observe(val, val2) { 
                if (JSON.stringify(val) !== JSON.stringify(val2)) {
                    $select.select2('destroy');
                    $select.select2(options);
                }
            });

            this.$fire("selmodel", _this.selmodel); 
        },
        onDispose: function () {
            $("#" + this.cid).select2('destroy');
        }
    }
});

avalon.component('ms-date', {
    template: ' <input ms-attr="{id:##cid}" ms-duplex="##value" class="bmdate form-control datepicker-input w120" type="text" >',
    defaults: {
        cid: "msd",
        value: 0,
        mclass: "",
        mdom: "",
        change: function () {

        },
        search: true,
        onInit: function () {
            this.cid += zutil.getguid();
        },
        onReady: function () {
            var _this = this;
            this.mdom = $("#" + this.cid);

            this.mdom.datetimepicker({
                format: 'yyyy-mm-dd',
                language: 'zh-CN',
                autoclose: true,
                weekStart: 1,
                startView: 2,
                minView: 2,
                forceParse: 0
            });
            this.$watch("value", function observe(val) {
                change(val);
            });


        }
    }
});

// <xmp ms-widget="{is:'ms-page',value:##count,change:pagechange}"></xmp>
avalon.component('ms-page', {
    template: '  <div  ms-attr="{id:##cid}"  ></div> ',
    defaults: {
        cid: "mspage",
        value: 0,
        mclass: "",
        mdom: "",
        curr: 1,
        change: avalon.noop,
        search: true,
        onInit: function () {
            this.cid += zutil.getguid();
        },
        onViewChange: function () {
            var _this = this;
            var first = true;
            laypage({
                cont: _this.cid,
                pages: this.value,//总页数
                skin: 'bm',
                skip: true,//是否开启跳页
                first: false,
                last: this.value,
                curr: _this.curr,
                prev: "<",
                next: ">",
                groups: 3,
                //total: data.length,
                jump: function (obj) {
                    if (!first) {
                        _this.change(obj.curr);
                        _this.curr = obj.curr;
                    } else {
                        first = false;
                    }
                }
            })
        },
        onReady: function () {
            var _this = this;
            this.mdom = $("#" + this.cid);
            this.$watch("value", function observe(size) {
                if (size) {
                    var first = true;
                    laypage({
                        cont: _this.cid,
                        pages: size,//总页数
                        skin: 'bm',
                        skip: true,//是否开启跳页
                        first: false,
                        last: size,
                        curr: _this.curr,
                        prev: "<",
                        next: ">",
                        groups: 3,
                        //total: data.length,
                        jump: function (obj) {
                            if (!first) {
                                _this.change(obj.curr);
                                _this.curr = obj.curr;
                            } else {
                                first = false;
                            }
                        }
                    })
                }

            });


        }
    }
});

//<xmp ms-widget="{is:'ms-modal',model:##detailmodal,title:detailmodal.title}"></xmp>
avalon.component('ms-modal', {
    template: '<div ms-attr="{id:##cid}"> <div ms-visible="##model.show" ><slot /></div> </div>',
    defaults: {
        cid: "modal",
        content: "",
        offset: "auto",
        model: { show: false },
        show: false,
        title: "查看",
        width: "500px",
        height: "auto",
        layerindex: 0,
        onInit: function () {
            this.cid += zutil.getguid();
        },
        onReady: function () {
            var _this = this;
            this.$watch("model.show", function observe(val) {
                if (val) {
                    _this.layerindex = layer.open({
                        type: 1,
                        offset: _this.offset,
                        zIndex: 1001,
                        title: [_this.title, " text-align:left;"],
                        area: [_this.width, _this.height],
                        cancel: function (index) {
                            _this.model.show = false;
                        },
                        content: $("#" + _this.cid)
                    });

                } else {
                    layer.close(_this.layerindex)
                }
            });


        }
    },
    soleSlot: "modal"
});


//<xmp ms-widget="{is:'ms-pagination',currentPage:1,totalCount:100,onPageClick:pageChange}"></xmp>
avalon.component('ms-pagination', {
    template: '<div class="vc-pagination" ms-if="totalPages>1">\
               <ul>\
                   <li ms-class="{none2:currentPage==1}">\
                       <a href="#" ms-click="firstPage()">首页</a>\
                   </li>\
                   <li ms-class="{none2:currentPage==1}">\
                       <a href="#" ms-click="prevPage()">上一页</a>\
                   </li>\
                   <li ms-for="page in pages">\
                       <a href="#" ms-class="{checked:currentPage==page}" ms-click="onPage(page)" ms-text="page"></a>\
                   </li>\
                   <li ms-class="{none2:currentPage==totalPages}">\
                       <a href="#" ms-click="nextPage()">下一页</a>\
                   </li>\
                   <li ms-class="{none2:currentPage==totalPages}">\
                       <a href="#" ms-click="endPage()">尾页</a>\
                   </li>\
                   <li>\
                       <span class="">\
                       <span>共</span>{{totalPages}}<span class="mg_r5">页</span>\
                       <span class="mg_r5">到第</span><input type="number" min="1" onkeyup="this.value=this.value.replace(/\D/, \'\');" class="w60 h20 lineH20" ms-duplex="skipPage"/><span class="mg_l5">页</span>\
                       <span class="mg_l5">总共</span>{{totalCount}}<span>条记录</span><a href="#" ms-click="skip(skipPage);">确定</a></span>\
                   </li>\
               </ul>\
           </div>',
    defaults: {
        currentPage: 1, 
        totalCount: 0,
        showPages: 10,
        pageSize:10,
        onPageClick: avalon.noop, 
        skipPage:"",
        firstPage: function () {
            this.currentPage = 1;
        },
        endPage: function () {
            this.currentPage = this.totalPages;
        },
        nextPage: function () {
            if (this.currentPage < this.totalPages) {
                this.currentPage++;
            }
            else {
                this.currentPage = this.totalPages;
            }
        },
        prevPage: function () {
            if (this.currentPage > 1) {
                this.currentPage--;
            } else {
                this.currentPage = 1;
            }
        },
        onPage: function (page) {
            if (this.currentPage != page) {
                this.currentPage = page;
            }
        },
        skip: function (page) {
            if (page != this.currentPage && page >= 1 && page <= this.totalPages) {
                this.currentPage = page-0;
            }
        },
        getPagination: function (currentPage, totalPages, showPage) {
            showPage = parseInt(showPage);
            var pages = [],
                start = Math.round(currentPage - showPage / 2),
                end = Math.round(currentPage + showPage / 2);
            if (end - start >= showPage) {
                start++;
            }
            if (start < 1) {
                start = 1;
                end = start + showPage - 1;
            }
            if (end >= totalPages) {
                end = totalPages;
                start = end - showPage + 1;
                if (start < 1) {
                    start = 1;
                }
            }
            for (var i = start; i <= end; i++) {
                pages.push(i);
            }
            return pages;
        },
        $computed: {
            totalPages: function () {
                return Math.ceil(this.totalCount / this.pageSize);
            },
            pages: function () {
                return this.getPagination(this.currentPage, this.totalPages, this.showPages);
            }
        },
        onReady: function () { 
            this.$watch('currentPage', function (newValue, oldValue) {
                if (newValue !== oldValue) { 
                    this.onPageClick(this.currentPage);
                }
            });
        }
    }
});

avalon.directive("positiveint", {
    init: function () {
        var _this = this;
        avalon.bind(this.node.dom, "keypress", function () {
            var inputCode = event.which,
            currentValue = this.value;
            if (inputCode > 0) {
                if (inputCode >= 48 && inputCode <= 57) {
                    var math = /^\d*.(\d*)$/.exec(currentValue);
                    return !(math && math[1].length >= 2 && _this.getCursorPosition(this) == currentValue.length);
                } else {
                    return inputCode == 8;
                }
            }
            return false;
        });
    },
    parse: function (cur, pre, binding) {
    },
    diff: function (copy, src, name) {
    },
    update: function (dom, vdom) {
    },
    getCursorPosition: function (element) {
        if (element.selectionStart) return element.selectionStart;
        else if (document.selection) {
            element.focus();
            var r = document.selection.createRange();
            if (r == null) return 0;

            var re = element.createTextRange(),
                rc = re.duplicate();
            re.moveToBookmark(r.getBookmark());
            rc.setEndPoint('EndToStart', re);
            return rc.text.length;
        }
        return 0;
    }
});