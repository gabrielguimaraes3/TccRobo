$(document).ready(function(){
    ListarProdutos();
})

var urlBaseApi = "https://localhost:7133";
var corpoTabelaProduto;


function LimparCorpoTabelaProdutos() {
    var componenteSelecionado = $('#corpoTabelaProduto tbody');
    componenteSelecionado.html('');
}

function ListarProdutos() {
    var rotaApi = '/robo';

    $.ajax({
        url: urlBaseApi + rotaApi,
        method: 'GET',
        dataType: "json"
    }).done(function (resultado) {
        ConstruirTabela(resultado);
    }).fail(function (err, errr, errrr) {

    });
}





function ConstruirTabela(linhas) {
    var htmlTabela = '';
  
    $(linhas).each(function (index, linha) {
      let precoAntigo = linha.precoAntigo || 0;
      let diferenca = (linha.preco - precoAntigo) / precoAntigo * 100;
      let diferencaFormatada = isNaN(diferenca) ? "N/A" : diferenca.toFixed(2) + "%";
  
      if (diferencaFormatada == 'Infinity%') {
        diferencaFormatada = 'Valor não encontrado';
      }
  
      htmlTabela += `<tr><th><a href="${linha.link}" target="_blank">${linha.nomeProduto}</a></th><td>${linha.preco}</td><td>${precoAntigo}</td><td>${diferencaFormatada}</td><td>${formatarData(linha.dataBusca)}</td></tr>`
    });
  
    $('#corpoTabelaProduto tbody').html(htmlTabela);
    $('#corpoTabelaProduto').DataTable();
  }


function formatarData(data) {

    let dataFormatada = new Date(data);
    let options = {
        day: "numeric",
        month: "numeric",
        year: "numeric"
    };
  
    return dataFormatada.toLocaleDateString("pt-BR", options);
}