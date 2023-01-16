﻿using ConversorMonetario;
using System.Collections;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using HttpClient cliente = new();

await ConverteMoeda(cliente);

static async Task<List<string>> SimbolosPermitidos(HttpClient cliente)
{
    var uri = "https://api.exchangerate.host/symbols";    

    var permitidos = await cliente.GetFromJsonAsync<Simbolos>(uri);

    return permitidos.Permitidos;
}

static async Task ConverteMoeda(HttpClient cliente)
{
    var simbolosPermitidos = await SimbolosPermitidos(cliente);
    
    var uri = new Controller().URIParaConversao(simbolosPermitidos);

    HttpResponseMessage response = await cliente.GetAsync(uri);
    new Controller().Status(response);

    if (!response.IsSuccessStatusCode) 
        await ConverteMoeda(cliente);

    Resultado resultado = await cliente.GetFromJsonAsync<Resultado>(uri);
    
    new Controller().MostrarResultados(resultado);
    await ConverteMoeda(cliente);
}