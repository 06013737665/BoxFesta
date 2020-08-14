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
   // qtde = $('#QtdeSelecionado').val();
    //$('.carrinho').html(qtde);
    $(".selectpicker2").change(function () {
        
        var valores = $("#valorKit").text().split('$');
        
        var valorKit = 0.00;
        if ($(this).val() === "10") {
            valorKit = parseFloat($("#ValorOriginalKit").val()).toFixed(2);
            $("#valorKit").text(valores[0] + '$' + valorKit);
        } else if ($(this).val() === "20") {
            valorKit = parseFloat(($("#ValorOriginalKit").val() * 2) - 5).toFixed(2);
            $("#valorKit").text(valores[0]+'$' + valorKit);
            
        } else if ($(this).val() === "30") {
            valorKit = parseFloat(($("#ValorOriginalKit").val() * 3) - 10).toFixed(2);
            $("#valorKit").text(valores[0] + '$' + valorKit);
        }
        else if ($(this).val() === "40") {
            valorKit = parseFloat(($("#ValorOriginalKit").val() * 4) - 15).toFixed(2);
            $("#valorKit").text(valores[0] + '$' + valorKit);
        } else if ($(this).val() === "50") {
            valorKit = parseFloat(($("#ValorOriginalKit").val() * 5) - 20).toFixed(2);
            $("#valorKit").text(valores[0] + '$' + valorKit);
        }
        
    });
});
function adicionarCarrinho(codigo)
{
    var quantidade = $(".qty").val();
    var pessoas = "0";
    if ($(".selectpicker2").val() !== undefined) {
        pessoas = $(".selectpicker2").val();
    }
    window.location.href = MontaCaminhoCompletoCart('Home/AdicionarProduto', codigo, quantidade) +"&pessoas="+pessoas;
    
}

function detalheProduto(codigo) {
    var arrayValor = "";
    for (var i = 0; i < j; i++) {
        var idString = "ProdutoSelecionado" + "_" + i + "_";
        arrayValor = arrayValor + $("#" + idString).val() + ",";

    }
    window.location.href = MontaCaminhoCompletoDetalhe('Home/DetalheProduto', codigo, arrayValor);
}
function MontaCaminhoCompletoDetalhe(caminho, codigo, produtosSacola) {
    var caminhoCompleto = MontaCaminhoCompleto(caminho);
    caminhoCompleto = caminhoCompleto + '?codigo=' + codigo + '&produtosSacola=' + produtosSacola;
    return caminhoCompleto;
}
function MontaCaminhoCompletoCart(caminho, codigo, quantidade) {
    var caminhoCompleto = MontaCaminhoCompleto(caminho);

    caminhoCompleto = caminhoCompleto + "?codigo=" + codigo+"&qtde="+quantidade;
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
