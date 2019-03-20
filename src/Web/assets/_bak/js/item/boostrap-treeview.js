$(function () {
    var $treeview = $('#treeview7');
    $.post("/Item/GetTree",
        { name: '1' },
        function (data) {

            TreeView($treeview, data);
        },
        "json");

    getEditHtml();
    $('#btn-edit').on('click', function (e) {
        e.preventDefault();
        var $this = this;
        var checkedvalue = $treeview.data("checkedvalue");
        var selectedvalue = $treeview.data("selectedvalue");
        var nodeselected = $treeview.data("nodeselected");
        var nodechecked = $treeview.data("nodechecked");

        //todo:各种判断没有增加
        var thisUrl = $(this).attr("href");
        var id = 0;
        if (checkedvalue !== "" && checkedvalue.indexOf(",") != -1) {
            var cvarr = checkedvalue.split(',');
            console.log(cvarr);
            if (cvarr.length != 3) {
                layer.msg("请选择一条数据");
                return false;
            }
            id = cvarr[1];

        } else {
            layer.msg("请至少选择一条数据");
            return false;
        }
        $.ajax({
            type: "get",
            url: thisUrl,
            data: { id: id, itemId: $("#hf-itemId").val() },
            success: function (data) {
                $(".sop-category-edit-model").html(data);
                $('.sop-modal-lg').modal('show');
            }
        });
        return false;

    });


    $('#btn-add').on('click', function (e) {
        e.preventDefault();
        var $this = this;
        var checkedvalue = $treeview.data("checkedvalue");
        var selectedvalue = $treeview.data("selectedvalue");
        var nodeselected = $treeview.data("nodeselected");
        var nodechecked = $treeview.data("nodechecked");
        //todo:各种判断没有增加
        //var objcheckvalue = JSON.parse(nodechecked);

        getEditHtml();
        $('.sop-modal-lg').modal('show');
        return false;
    });

    $('#btn-delete').on('click', function (e) {
        var $this = this;
        e.preventDefault;
        var checkedvalue = $treeview.data("checkedvalue");
        var selectedvalue = $treeview.data("selectedvalue");
        var nodeselected = $treeview.data("nodeselected");
        var nodechecked = $treeview.data("nodechecked");
        //todo:各种判断没有增加
        var objcheckvalue = JSON.parse(nodechecked);
        var ids = checkedvalue;
        console.log(ids);
        if (ids == "" || ids == ",") {
            layer.msg("请至少选择一条删除数据");
            return false;
        }

        var thisUrl = $(this).attr("href");

        layer.confirm('确认删除吗？', {
            btn: ['确认', '取消'] //按钮
        }, function () {
            $.post(thisUrl, { ids: ids }, function (data) {
                console.log(data);
                if (data.type == "@((int)SystemMessageType.Success)") {
                    layer.msg(data.content);
                }
                window.location.reload();
            });

        }, function () {
            layer.msg('取消操作');
        });

        return false;



    });


    //CatalogDelete
    //var status = $("btn-edit").attributes["data-status"].value;
    //if (status) {
    //    $('.modal-title').text('New message to ' + node.text);
    //    $('#exampleModal').modal({
    //        keyboard: true
    //    });
    //}
});

function TreeView($treeview, data) {
    var $tree = $treeview.treeview({
        color: "#428bca",
        levels: 99,
        expandIcon: 'fa fa-chevron-right',
        collapseIcon: 'fa fa-chevron-down',
        nodeIcon: "fa fa-file",
        showTags: true,
        showBorder: true,
        enableLinks: true,
        showIcon: true,
        showCheckbox: true,
        data: data,
        multiSelect: false,
        onNodeSelected: function (event, node) {
            var selectedvalue = $treeview.data("selectedvalue");
            if (selectedvalue === "") {
                selectedvalue = "," + node.id + ",";
            } else {
                if (selectedvalue.toString().indexOf(node.id) === -1) {
                    selectedvalue = selectedvalue + node.id + ",";
                }
            }
            var str = JSON.stringify(node);
            if ($treeview.data) {
                $treeview.data("selectedvalue", selectedvalue);
                $treeview.data("nodeselected", str);
            }
            console.log('<p>' + node.text + ' was onNodeSelected</p>');
        },
        onNodeUnselected: function (event, node) {
            var selectedvalue = $treeview.data("selectedvalue");
            if (selectedvalue !== "") {
                var reg = "";
                if (selectedvalue.toString().indexOf("," + node.id + ",") != -1) {
                    reg = "," + node.id + ",";
                }
                selectedvalue = selectedvalue.toString().replace(reg, ',');
            }
            $treeview.data("selectedvalue", selectedvalue);
            console.log('<p>' + node.text + ' was onNodeUnselected</p>');
        },
        onNodeChecked: function (event, node) {
            var checkedvalue = $treeview.data("checkedvalue");
            if (checkedvalue === "") {
                checkedvalue = "," + node.id + ",";
            } else {
                if (checkedvalue.toString().indexOf(node.id) === -1) {
                    checkedvalue = checkedvalue + node.id + ",";
                }
            }
            var str = JSON.stringify(node);
            if ($treeview.data) {
                $treeview.data("checkedvalue", checkedvalue);
                $treeview.data("nodechecked", str);
            }
            console.log('<p>' + node.text + ' was onNodeChecked</p>');
        },
        onNodeUnchecked: function (event, node) {
            var checkedvalue = $treeview.data("checkedvalue");
            if (checkedvalue !== "") {
                var reg = "";
                if (checkedvalue.toString().indexOf("," + node.id + ",") != -1) {
                    reg = "," + node.id + ",";
                }
                checkedvalue = checkedvalue.toString().replace(reg, ',');
            }
            $treeview.data("checkedvalue", checkedvalue);
            console.log('<p>' + node.text + ' was onNodeUnchecked</p>');
        },
        onNodeDisabled: function (event, node) {
            console.log('<p>' + node.text + ' was onNodeDisabled</p>');
        },
        onNodeEnabled: function (event, node) {
            console.log('<p>' + node.text + ' was onNodeEnabled</p>');
        },
        onNodeCollapsed: function (event, node) {
            console.log('<p>' + node.text + ' was onNodeCollapsed</p>');
        }
    });
    var silent1 = false;
    //启用禁用
    $('#btn-disable-all').on('click', function (e) {
        var $this = this;
        var status = $this.attributes["data-status"].value;
        console.log(status);
        if (status == "true") {
            $this.attributes["data-status"].value = false;
            $this.innerText = "启用";
            $tree.treeview('disableAll', { silent: silent1 });
        }
        if (status == "false") {
            $this.attributes["data-status"].value = true;
            $this.innerText = "禁用";
            $tree.treeview('enableAll', { silent: silent1 });
        }


    });

    $('#btn-check-all').on('click', function (e) {
        var $this = this;
        var status = $this.attributes["data-status"].value;
        console.log(status);
        if (status == "true") {
            $this.attributes["data-status"].value = false;
            $this.innerText = "取消";
            $tree.treeview('checkAll', { silent: silent1 });
        }
        if (status == "false") {
            $this.attributes["data-status"].value = true;
            $this.innerText = "全选";
            $tree.treeview('uncheckAll', { silent: silent1 });
        }


    });

    $('#btn-expand-all').on('click', function (e) {
        var $this = this;
        var status = $this.attributes["data-status"].value;
        console.log(status);
        if (status == "true") {
            $this.attributes["data-status"].value = false;
            $this.innerText = "收缩";

            $tree.treeview('expandAll', { silent: silent1 });
        }
        if (status == "false") {
            $this.attributes["data-status"].value = true;
            $this.innerText = "展开";

            $tree.treeview('collapseAll', { silent: silent1 });
        }


    });

    var results = function () {
        var pattern = $('#input-search').val();
        var options = {
            ignoreCase: true,
            exactMatch: false,
            revealResults: true
        };
        return $tree.treeview('search', [pattern, options]);
    }
    var search = function (e) {
        results();
        var output = '<p>' + results.length + ' matches found</p>';
        $.each(results, function (index, result) {
            output += '<p>- ' + result.text + '</p>';
        });
        console.log(output);
        //$tree.treeview('selectNode', [results(), { silent: silent1 }]);

    }

    $('#btn-search').on('click', search);
    $('#input-search').on('keyup', search);

    $('#btn-clear-search').on('click', function (e) {
        $tree.treeview('clearSearch');
        //$tree.treeview('toggleNodeSelected', [results(), { silent: silent1 }]);
        //$tree.treeview('unselectNode', [results(), { silent: silent1 }]);
        $('#input-search').val('');

    });




}




function getEditHtml() {
    $.ajax({
        type: "get",
        url: "@SiteUrls.Instance().ItemCatalogEdit()",
        async: true,
        data: { itemId: $("#hf-itemId").val() },
        success: function (data) {
            $(".sop-category-edit-model").html(data);
        }
    });

}