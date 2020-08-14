var qtde = 0;
var j = 0;
var valorCarrinho = 0.00;
var produtos = [];
$(document).ready(function ($) {

    // $('.add_cart_btn').click(function () {
    //     alert("Carrinho");
    // });
    $('.loader2').hide();
    $('.loader3').hide();
    qtde = $('#QtdeSelecionado').val();
    $('.carrinho').html(qtde);
    $('#zip').keypress(function () {
       $(this).mask('00000-000');
    });
    $('.qtdeText').keypress(function () {
        $(this).mask('00');
    });
    var codePagseguro = $('#CodePagSeguro').val();
    if (codePagseguro !== undefined && codePagseguro !== '') {
        PagSeguroLightBox(codePagseguro);
    }
});

function PagSeguroLightBox(codePagseguro){
    var code = codePagseguro;
    var callback = {
        success: function (transactionCode) {
            //Insira os comandos para quando o usuário finalizar o pagamento. 
            //O código da transação estará na variável "transactionCode"
            var retorno = MontaCaminhoCompleto('Home/FinalizarPedido') + "?id_transacao=" + transactionCode;
            location.href = retorno;
            console.log("Compra feita com sucesso, código de transação: " + transactionCode);
        },
        abort: function () {
            //Insira os comandos para quando o usuário abandonar a tela de pagamento.
           
            console.log("abortado");
        }
    };
    //Chamada do lightbox passando o código de checkout e os comandos para o callback
    var isOpenLightbox = PagSeguroLightbox(code, callback);
    // Redireciona o comprador, caso o navegador não tenha suporte ao Lightbox
    if (!isOpenLightbox) {
       // location.href = "https://pagseguro.uol.com.br/v2/checkout/payment.html?code=" + code;
        location.href = "https://sandbox.pagseguro.uol.com.br/v2/checkout/payment.html?code=" + code;
    }
}

function excluirProduto(codigo) {
    window.location.href = MontaCaminhoCompletoCart('Home/ExcluirProduto', codigo);
}

function atualizarQuantidade(codigo) {
    var id = "#qtde_" + codigo;
    var qtde = $(id).val()+"";
    
    window.location.href = MontaCaminhoCompletoQuantidade('Home/AtualizarQuantidade', codigo, qtde);
}

function MontaCaminhoCompletoQuantidade(caminho, codigo, qtde) {
    var caminhoCompleto = MontaCaminhoCompleto(caminho);

    caminhoCompleto = caminhoCompleto + "?codigo=" + codigo + "&qtde=" + qtde;
    return caminhoCompleto;
}

function MontaCaminhoCompletoCart(caminho, codigo) {
    var caminhoCompleto = MontaCaminhoCompleto(caminho);

    caminhoCompleto = caminhoCompleto + "?codigo=" + codigo;
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
