
function isValidEmail(email) {
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    return emailReg.test(email);
}

var cancelLoading = false;

function desableLoading() {
    cancelLoading = true;
}

function loading() {

    if (cancelLoading)
        return;

    if ($(".modal.open").length)
        $(".modal.open").prepend(getHtmlLoading());
    else
        $("body").prepend(getHtmlLoading());

    $('.preloader-background, .preloader-wrapper').fadeIn();
};

function noLoading() {

    if (cancelLoading) {
        cancelLoading = false;
        return;
    }

    $('.preloader-background, .preloader-wrapper').fadeOut('slow');
    $(".preloader-background").remove();
};

function getHtmlLoading() {
    
    return  '<div class="preloader-background">'
        +      '<div class="preloader-wrapper big active">'
        +          '<div class="spinner-layer spinner-blue-only">'
        +              '<div class="circle-clipper left">'
        +                  '<div class="circle"></div>'
        +              '</div>'
        +              '<div class="gap-patch">'
        +                  '<div class="circle"></div>'
        +              '</div>'
        +              '<div class="circle-clipper right">'
        +                  '<div class="circle"></div>'
        +              '</div>'
        +          '</div>'
        +      '</div>'
        +  '</div>';
}

$.toast = function(config) {
    var cfg = $.extend({
        condition: true,
        message: "",
        type: "error",
        delay: 10000
    }, config);

    if (!cfg.condition)
        return cfg.condition;

    var color;
    switch (cfg.type) {
        case "success": color = "green"; break;
        default: color = "red"; break;
    }
    Materialize.toast("<span>" + cfg.message + "</span>", cfg.delay, color);

    return cfg.condition;
}

function isFieldEmpty(fields) {

    var hasFieldEmpty = false;

    for (var i = 0; i < fields.length; i++) {

        if ($(fields[i]).val())
            continue;

        var fieldId = $(fields[i]).attr("id");
        var labelName = $("label[for='" + fieldId + "']").text();
        $.toast({ message: "Favor preencher o campo " + labelName });
        hasFieldEmpty = true;
    }

    return hasFieldEmpty;
}

$(document).ajaxStart(loading).ajaxStop(noLoading).ajaxError(function (event, xhr) {
    $.toast({message: xhr.responseText || "Falha ao realizar operação"});
});

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

var onModalConfirm;

function showModalConfirm(mensagem, fnc) {
    onModalConfirm = fnc;
    var htmlModal = '<div id="modalConfirmacao" class="modal">'
                    +    '<div class="modal-content">'
                    +       "<br/>"
                    +       "<h4>Confirmação</h4>"
                    +       "<div class='divider'></div>"
                    +       "<a style='position: absolute; top: 10px; right: 50px;'>"
                    +           "<i class='large material-icons' style='color: gold;'>warning</i>"
                    +        "</a>"
                    +       "<p>" + mensagem + "</p>"
                    +    "</div>"
                    +    "<div class='modal-footer'>"
                    +       "<a href='#!' onclick='onModalConfirm()' style='color:green' class='modal-action modal-close waves-effect waves-green btn-flat'>Confirmar</a>"
                    +       "<a href='#!' style='color:red' class='modal-action modal-close waves-effect waves-red btn-flat'>Cancelar</a>"
                    +    "</div>"
                    + " </div>";

    $("#containerPrincipal").prepend(htmlModal);
    $(".modal").modal();
    $("#modalConfirmacao").modal("open");
}

function verifyLabelActive() {
    $("label").each(function (e, element) {
        var id = $(element).attr("for");
        if ($("#" + id).val())
            $(element).addClass("active");
    });
}


String.prototype.toDecimal = function () {
    return (this == "" ? 0 : parseFloat(this.replace(/\./g, '').replace(',', '.')));
}

Number.prototype.toMoney = function () {
    var re = '\\d(?=(\\d{3})+\\D)',
    num = this.toFixed(2);
    return num.replace('.', ',').replace(new RegExp(re, 'g'), '$&' + ('.'));
};


function sendAjaxAtualizaResumoGeral(url, idConta, idCategoria) {
    $.get(url, {
        idConta: idConta,
        idCategoria: idCategoria
    }).success(function (json) {
        $("#spanSaldoPrev").text(json.TotalSaldoPrevisto);
        $("#spanSaldoAtual").text(json.TotalSaldoAtual);
        $("#liDespesaPrev").text(json.TotalDespesasPrevista);
        $("#liDespesaRealiz").text(json.TotalDespesasRealizada);
        $("#liReceitaPrev").text(json.TotalReceitasPrevista);
        $("#liReceitaRealiz").text(json.TotalReceitasRealizada);
        // atualiza a cor do saldo (vermelho ou verde)
        $("#spanSaldoAtual").css("color", (json.TotalSaldoAtual.toDecimal() >= 0) ? "greenyellow" : "red");
    });
}
