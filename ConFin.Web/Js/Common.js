

// fazer plugin para ser usado no sistema

var loading = (function loading() {
    $('.preloader-background, .preloader-wrapper').fadeIn();
    return loading;
})();

var noLoading = (function noLoading() {
    $('.preloader-background, .preloader-wrapper').fadeOut('slow');
    return noLoading;
})();

$(document).ajaxStart(loading).ajaxStop(noLoading);

jQuery.fn.extend({
    toObject: function () {
        var obj = {}, formArray = [];

        if ($(this).is("form"))
            formArray = this.serializeArray();
        else {
            var container = $(this).wrap("<form/>");
            formArray = container.parent().serializeArray();
            container.unwrap("<form/>");
        }

        if (!formArray.length)
            return obj;

        $.each(formArray, function () {
            if (obj[this.name] === undefined)
                obj[this.name] = this.value || '';
            else {
                if (!obj[this.name].push)
                    obj[this.name] = [obj[this.name]];
                obj[this.name].push(this.value || '');
            }
        });

        return obj;
    }
});