//for open

function OpenCustomDialog(dialogUrl, dialogWidth, dialogHeight, dialogTitle,


dialogAllowMaximize, dialogShowClose) {


    CloseProgressPopUp();


    var options = {
        url: dialogUrl,
        allowMaximize: dialogAllowMaximize,
        showClose: dialogShowClose,
        width: dialogWidth,
        height: dialogHeight,
        title: dialogTitle,
        dialogReturnValueCallback: Function.createDelegate(null, CloseCallback3)
    };
    SP.UI.ModalDialog.showModalDialog(options);
}

//for close
window.frameElement.commitPopup();