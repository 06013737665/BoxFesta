/* =================================
------------------------------------
	The Plaza - eCommerce Template
	Version: 1.0
 ------------------------------------ 
 ====================================*/


'use strict';


$(window).on('load', function() {
	/*------------------
		Preloder
	--------------------*/
	$(".loader").fadeOut(); 
	$("#preloder").delay(400).fadeOut("slow");


	/*------------------
		Product filter
	--------------------*/
	if($('#product-filter').length > 0 ) {
		var containerEl = document.querySelector('#product-filter');
		var mixer = mixitup(containerEl);
	}

});

(function($) {
	/*------------------
		Navigation
	--------------------*/
	$('.nav-switch').on('click', function(event) {
		$('.main-menu').slideToggle(400);
		event.preventDefault();
	});


	/*------------------
		Background Set
	--------------------*/
	$('.set-bg').each(function() {
		var bg = $(this).data('setbg');
		$(this).css('background-image', 'url(' + bg + ')');
	});


	/*------------------
		Hero Slider
	--------------------*/
	 


	/*------------------
		Intro Slider
	--------------------*/
	if($('.intro-slider').length > 0 ) {
		var $scrollbar = $('.scrollbar');
		var $frame = $('.intro-slider');
		var sly = new Sly($frame, {
			horizontal: 1,
			itemNav: 'forceCentered',
			activateMiddle: 1,
			smart: 1,
			activateOn: 'click',
			//mouseDragging: 1,
			touchDragging: 1,
			releaseSwing: 1,
			startAt: 10,
			scrollBar: $scrollbar,
			//scrollBy: 1,
			activatePageOn: 'click',
			speed: 200,
			moveBy: 600,
			elasticBounds: 1,
			dragHandle: 1,
			dynamicHandle: 1,
			clickBar: 1,
		}).init();
	}



	/*------------------
		ScrollBar
	--------------------*/
	$(".cart-table, .product-thumbs").niceScroll({
		cursorborder:"",
		cursorcolor:"#afafaf",
		boxzoom:false
	});



	/*------------------
		Single Product
	--------------------*/
	$('.product-thumbs-track > .pt').on('click', function(){
		var imgurl = $(this).data('imgbigurl');
		var bigImg = $('.product-big-img').attr('src');
		if(imgurl != bigImg) {
			$('.product-big-img').attr({src: imgurl});
		}
	})

})(jQuery);

function MostrarMensagem(fonte, tipo, valor, classAdicional) {
    if (classAdicional == null || classAdicional == undefined) {
        classAdicional = "";
    }

    var blocoMensagem = $("#mensagem" + fonte);

    blocoMensagem.html("<div class='fade-alert alert alert-" + tipo + " " + classAdicional + "'><a class='close' onclick='$(this).parent().slideUp(1000, function(){ $(this).children().remove(); })'>&times;</a><span></span></div>").slideDown();

    if (valor == undefined || valor == null) {
        valor = "Houve um erro inesperado no retorno da execução."
    }
    if (valor.length > 1 && typeof valor != "string") {
        blocoMensagem.find("span").append('<ul></ul>');

        for (var i = 0; i < valor.length; i++) {
            blocoMensagem.find("ul").append('<li>' + valor[i] + '</li>');
        }
    }
    else {
        blocoMensagem.find("span").html(valor);
    }

    if (tipo == "success") {
        timeoutID = window.setTimeout(function () {
            blocoMensagem.slideUp(1000, function () { $(this).children().remove(); });
        }, 10000);
    } /*else if (tipo == "warning") {
        timeoutID = window.setTimeout(function () {
            blocoMensagem.slideUp(1000, function () { $(this).children().remove(); });
        }, 10000);
    }*/ else if (typeof timeoutID != 'undefined' && timeoutID !== undefined) {
        window.clearTimeout(timeoutID);
    }

    $.scrollTo("#mensagem" + fonte, 400, { offset: -30 });

    // Retorno da referência do bloco que contém as mensagens para posteriores utilizações
    return blocoMensagem;
}