var qtde = 0;
var j = 0;
var valorCarrinho = 0.00;
var produtos = [];
$(document).ready(function ($) {

    // $('.add_cart_btn').click(function () {
    //     alert("Carrinho");
    // });
    
    if ($(".alert-danger").length!=1) {
        $('#nome').val("");
        $('#cpf').val("");
        $('#NovoCpf').val("");
        $('#senha').val("");
        $('#NovaSenha').val("");
        $('#ConfirmarSenha').val("");
        $('#tel').val("");
        $('#email').val("");
    }
    $('#cpf').mask('000.000.000-00');
    $('#cpf').keypress(function () {
        $(this).mask('000.000.000-00');
    });
    $('#NovoCpf').mask('000.000.000-00');
    $('#NovoCpf').keypress(function () {
        $(this).mask('000.000.000-00');
    });

    $('#tel').mask('(00)00000-0000');
    $('#tel').keypress(function () {
        $(this).mask('(00)00000-0000');
    });


    $('.loader2').hide();
    $('.loader3').hide();
    qtde = $('#QtdeSelecionado').val();
    $('.carrinho').html(qtde);
  
    
});

function excluirProduto(codigo) {
    window.location.href = MontaCaminhoCompletoCart('Home/ExcluirProduto', codigo);
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
