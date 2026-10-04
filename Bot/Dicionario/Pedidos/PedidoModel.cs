using System;

namespace ChatBotTelegram.Bot.Dicionario.Pedidos;

public class PedidoModel
{
    public String[] values;
    public String pedido_name;
    public double preco = 0; 
    public bool pedido_finalizado = false;
    public int i_value = 0;

    public PedidoModel(int size_values,String pedido_name) // definindo tamanho de "values"
    {
    this.values = new String[size_values];
    this.pedido_name = pedido_name;
    }
    

    public String retornarArrayAsString<T>(T[] array)
    {
    String texto_retorno = "";
        for(int i=0;i<array.Length;i++)
        {
        T i_item = array[i];
        texto_retorno += i_item.ToString();
            if(i+1 < array.Length)
            {
            texto_retorno += ", ";
            }
        }
    return texto_retorno;
    }

}
