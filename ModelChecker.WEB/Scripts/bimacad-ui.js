

function UpdateResult() {
    SubmitForm('UpdateForm');
}


function SubmitForm(formId) {
    var _form = document.getElementById(formId);
    if (_form != null) {
        var submit = _form.querySelector('button[type="submit"]');
        if (submit != null)
            submit.click();
    }
}


function CheckAll(e, block) {
    var _block = document.getElementById(block);
    if (_block != null) {
        var tbody = _block,
            cboxes = tbody.getElementsByTagName('input');
        for (var i = 0; i < cboxes.length; i++) {
            cboxes[i].checked = e.checked;
        }
    }
}

//function CheckAllRefresh(e, block) {
//    CheckAll(e, block);
//    if (e.checked == true) {
//        UpdateChild();
//    }
    
//}

function ChangeChackAll(block, parent) {
    var _par = document.getElementById(parent);

    if (_par != null) {
        var _ch = true;
        var _block = document.getElementById(block);
        var tbody = _block,
            cboxes = tbody.getElementsByTagName('input');
        for (var i = 0; i < cboxes.length; i++) {
            if (cboxes[i].checked == false) {
                _ch = false;
            }
        }
        _par.checked = _ch;
    }
}


function SetDisabled(block, elem) {
    var _block = document.getElementById(block);
    if (_block != null) {
        var tbody = _block,
            cboxes = tbody.getElementsByTagName('input');
        for (var i = 0; i < cboxes.length; i++) {
            if (cboxes[i].checked == true) {
                document.getElementById(elem).removeAttribute("disabled");
                return;
            }
        }
        document.getElementById(elem).setAttribute("disabled", "disabled");
    }
}