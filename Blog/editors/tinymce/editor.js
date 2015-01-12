var editorGetHtml = function () {
    return tinymce.activeEditor.getContent();
}

var editorSetHtml = function (html) {
    tinymce.activeEditor.setContent(html);
}

tinymce.init({
    selector: '#txtContent',
    plugins: [
        "advlist autolink lists link image charmap print preview anchor",
        "searchreplace visualblocks code fullscreen",
        "insertdatetime media table contextmenu paste sh4tinymce"
    ],
    toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | sh4tinymce",
    autosave_ask_before_unload: false,
    max_height: 400,
    min_height: 160,
    height: 280
});
