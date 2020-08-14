
$(document).ready(function ($) {
    $('#data').keypress(function () {
        $(this).mask('00/00/0000');
    });
});


function MontaCaminhoCompletoQuantidade(caminho, codigo, qtde) {
    var caminhoCompleto = MontaCaminhoCompleto(caminho);

    caminhoCompleto = caminhoCompleto + "?codigo=" + codigo + "&qtde=" + qtde;
    return caminhoCompleto;
}

function MontaCaminhoCompletoCart(caminho) {
    var caminhoCompleto = MontaCaminhoCompletoAdmin(caminho);
 
    return caminhoCompleto;
}

function MontaCaminhoCompletoAdmin(caminho) {
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
        if (tokens[i] === '' ) {
            continue;
        }

        tokenCorrente = tokens[i];
        caminhoCompleto += '/' + tokens[i];
    }

    return caminhoCompleto;
}
