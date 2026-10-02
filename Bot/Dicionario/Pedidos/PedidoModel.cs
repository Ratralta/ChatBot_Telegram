using System;

namespace ChatBotTelegram.Bot.Dicionario.Pedidos;

public class PedidoModel
{
    public String[] values;

    /*
    public PedidoModel(int values_size) // definindo tamanho de "values"
    {
    this.values = new String[values_size];
    }
    */

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
