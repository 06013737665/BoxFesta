//"use strict";

//var $window, $document, $body;

//$window = $(window);
//$document = $(document);
//$body = $("body");


var qtde = 0;
var j = 0;
var valorCarrinho = 0.00;
var produtos = [];

$(document).ready(function () {

    // $('.add_cart_btn').click(function () {
    //     alert("Carrinho");
    // });
    qtde = $('#QtdeSelecionado').val();
    valorCarrinho = $('#ValorSelecionado').val();
    valorCarrinho = parseFloat(valorCarrinho).toFixed(2);
    $('.carrinho').html(qtde);
    $('#price').html(valorCarrinho);
    $("#textoHeader2").fadeOut();
    $(".loader2").hide();
    $("#preloader2").fadeOut("slow");
    
    var secao = $('#Secao').val();

    if (typeof secao !== 'undefined' && secao !== '') {

        if (secao === "2") {

            window.location.href = '#secao2';
        }
        else if (secao === "3") {
            window.location.href = '#secao3';
        } else if (secao === "1") {
            window.location.href = '#secao1';
        }
    }

    $("#textoHeader1").fadeIn(2000);
    sumirTexto();

    $(".item").mouseover(function () {
        $(".lupa_kit").css('display', 'block');
  
    });
    $(".item").mouseout(function () {
        $(".lupa_kit").css('display', 'none');
    });
    $(".from_blog_item").mouseover(function () {
        var teste = $(this).find(".portfolio-slider-alt").data('flexslider');
        teste.play();
       
    });

    $(".from_blog_item").mouseout(function () {
        var teste = $(this).find(".portfolio-slider-alt").data('flexslider');
        teste.pause();
       
        
    });
        
   
    /*==============================================
         Flex slider init
         ===============================================*/
    
        $(".portfolio-slider").flexslider({
            animation: "slide",
            direction: "vertical",
           
            slideshowSpeed: 3000,
            start: function () {
                imagesLoaded($(".portfolio"), function () {
                    setTimeout(function () {
                        $(".portfolio-filter li:eq(0) a").trigger("click");
                    }, 500);
                });
            }
        });

    

  /* $window.load(function () {
        $(".portfolio-slider-alt").flexslider({
            animation: "slide",
            direction: "horizontal",
            slideshow: false,  
            slideshowSpeed: 4000,
            start: function () {
                imagesLoaded($(".portfolio"), function () {
                    setTimeout(function () {
                        $(".portfolio-filter li:eq(0) a").trigger("click");
                    }, 500);
                });
            }
        });

    });*/
    $(".portfolio-slider-alt").flexslider({
            animation: "slide",
            direction: "horizontal",
            slideshow: false,
            slideshowSpeed: 2000,
            start: function (slider) {
                slider.pause();
                slider.manualPause = true;
             }
        });

    

         $(".post-slider-thumb").flexslider({
            animation: "slide",
            controlNav: "thumbnails"
        });
     

         $(".post-slider").flexslider({
            animation: "slide"
            //slideshow: false
        });
     

         $(".news-slider").flexslider({
            animation: "slide",
            slideshowSpeed: 3000
        });
    

    /*==============================================
         Portfolio item slider init
         ===============================================*/
    $(".portfolio-slider, .portfolio-slider-alt").each(function () { // the containers for all your galleries
        var _items = $(this).find("li > a");
        var items = [];
        for (var i = 0; i < _items.length; i++) {
            items.push({ src: $(_items[i]).attr("href"), title: $(_items[i]).attr("title") });
        }
        $(this).parent().find(".action-btn").magnificPopup({
            items: items,
            type: "image",
            gallery: {
                enabled: true
            }
        });
        $(this).parent().find(".portfolio-description").magnificPopup({
            items: items,
            type: "image",
            gallery: {
                enabled: true
            }
        });
    });


    // $(".img-kit").each(function () {
    // fadeOut(this, 1);   

    //     });
});


function sumirTexto() {
    $("#textoHeader1").fadeIn(2000, function (event) {
        setTimeout(3000);
        $("#textoHeader1").fadeOut(5000);

    });
    $("#textoHeader1").fadeOut(5000, function (event) {
        setTimeout(1000);
        $("#textoHeader2").fadeIn(2000, function (event) {

            setTimeout(3000);
            $("#textoHeader2").fadeOut(5000, function (event) {
                setTimeout(1000);
                $("#textoHeader1").css("display", "none");
                $("#textoHeader1").fadeIn(2000, function (event) {

                    setTimeout(3000);
                    sumirTexto();
                });
            });
        });

    });
}
function detalheProduto(codigo) {
    var arrayValor = "";
    for (var i = 0; i < j; i++) {
        var idString = "ProdutoSelecionado" + "_" + i + "_";
        arrayValor = arrayValor + $("#" + idString).val() + ",";

    }
    window.location.href = MontaCaminhoCompletoDetalhe('Home/DetalheProduto', codigo, arrayValor);
}
function selecionarCarrinho(id, codigo, valor) {
    $("#preloader2").fadeIn("slow");
    $(".loader2").show();
    
    setTimeout(function () {
    qtde = $('#QtdeSelecionado').val();
    qtde = parseInt(qtde) + 1;
    valorCarrinho = parseFloat(valorCarrinho) + parseFloat(valor);
    valorCarrinho = valorCarrinho.toFixed(2);
    $('#QtdeSelecionado').val(qtde);
    $('.carrinho').html(qtde);
    $('#ValorSelecionado').val(valorCarrinho);
    $('#price').html(valorCarrinho);

    var idString = "ProdutoSelecionado" + "_" + j + "_";
    var elementoArray = $("#" + idString);
    elementoArray.val(codigo);
    j++;
        
        $(".loader2").hide();
        $("#preloader2").fadeOut("slow");
      
    }, 1000);

    
    // window.location.href = window.location.origin + MontaCaminhoCompleto('Home/Index#lancamentos');

}
function MontaCaminhoCompletoDetalhe(caminho, codigo, produtosSacola) {
    var caminhoCompleto = MontaCaminhoCompleto(caminho);
    caminhoCompleto = caminhoCompleto + '?codigo=' + codigo + '&produtosSacola=' + produtosSacola;
    return caminhoCompleto;
}
function MontaCaminhoCompletoCart(caminho) {
    var caminhoCompleto = MontaCaminhoCompleto(caminho);
    var ArrayValor = "";
    for (var i = 0; i < j; i++) {
        var idString = "ProdutoSelecionado" + "_" + i + "_";
        ArrayValor = ArrayValor + $("#" + idString).val() + ",";

    }
    caminhoCompleto = caminhoCompleto + '?listaProduto=' + ArrayValor;

    return caminhoCompleto;
}
function MontaCaminhoCompletoPesquisa(caminho) {
    var caminhoCompleto = MontaCaminhoCompleto(caminho);
    var ArrayValor = "";
    for (var i = 0; i < j; i++) {
        var idString = "ProdutoSelecionado" + "_" + i + "_";
        ArrayValor = ArrayValor + $("#" + idString).val() + ",";

    }
    caminhoCompleto = caminhoCompleto + '?listaProduto=' + ArrayValor + '&buscar=' + $('#textoBuscar').val();

    return caminhoCompleto;
}
function MontaCaminhoCompletoProducts(caminho, secao) {
    var caminhoCompleto = MontaCaminhoCompleto(caminho);
    var ArrayValor = "";
    for (var i = 0; i < j; i++) {
        var idString = "ProdutoSelecionado" + "_" + i + "_";
        ArrayValor = ArrayValor + $("#" + idString).val() + ",";

    }
    caminhoCompleto = caminhoCompleto + '?listaProduto=' + ArrayValor + '&secao=' + secao;

    return caminhoCompleto;
}

function MontaCaminhoCompleto(caminho) {
    // Remover todas as possíveis repetições de barras de formação da url (ex: "//Home/AcessoInicial?tipoEmpregador=geral")
    var caminhoCompleto = ($('#HiddenCurrentUrl').val() + caminho).replace(/\/+/g, '/');

    // Partir a url em tokens com o separador "/",
    // remover as duplicações sucessivas (ex: "/portalweb-completo-003/portalweb-completo-003/FolhaPagamento/Listagem/ListarPagamentos?competencia=201707")
    // e montá-la novamente
    var tokens = caminhoCompleto.split('/');
    var tokenCorrente = null;
    var i = 0;

    caminhoCompleto = '';

    for (; i < tokens.length; ++i) {
        if (tokens[i] == '' || tokenCorrente == tokens[i]) {
            continue;
        }

        tokenCorrente = tokens[i];
        caminhoCompleto += '/' + tokens[i];
    }

    return caminhoCompleto;
}


