using System;
using System.Globalization;

namespace ChatBotTelegram.Bot.Dicionario;

public class Dicionario_Bot
{
public enum intencoes_enum
{
    saudacao,
}

public Dictionary<String,String> intencoes_dicionario;
public Dictionary<String,String> sabores_pizza;
    public Dicionario_Bot(String[] sabores)
    {
    this.intencoes_dicionario = new Dictionary<String, String>()
    {
        {"ola", intencoes_enum.saudacao.ToString()},
        {"bom dia", intencoes_enum.saudacao.ToString()},
        {"boa noite", intencoes_enum.saudacao.ToString()},
        {"gostaria", intencoes_enum.saudacao.ToString()},
        {"quero",intencoes_enum.saudacao.ToString()}
    };
    this.sabores_pizza = new Dictionary<String, String>(){}; // adicionando sabor em cada index do parâmetro "sabores[]"
        for(int i=0;i<sabores.Length;i++) 
        {
        String i_sabor = sabores[i].ToLower().Trim(); // tratando para adicionar ao dicionario
        this.sabores_pizza.Add(i_sabor,i_sabor);
        }

    }
    public String[]? tryGetSabores(String texto)
    {
    List<String> sabores_list_return = new List<String>(); // lista para retorno 
    int limite_de_sabores = 2; // maximo de sabores numa pizza 

    String[] texto_tratado_array = texto.ToLower().Trim().Split(" "); // tratando "texto" 

        for(int i=0;i<texto_tratado_array.Length;i++)
        {
        bool stop_for_bool = false; // sai do for loop se for true 

            if(sabores_list_return.Count >= limite_de_sabores){stop_for_bool = true;} // se passar do limite de sabores


            if(!stop_for_bool)  
            {
            String i_palavra = texto_tratado_array[i];
                if(this.sabores_pizza.ContainsValue(i_palavra))
                {
                sabores_list_return.Add(i_palavra);
                } 
            }
            else{i = texto_tratado_array.Length;}
        }
    if(sabores_list_return.Count == 0){return null;} // retorna null se lista não tiver nenhum item 
    return sabores_list_return.ToArray(); // retorna como array se tiver algo
    }
    public Dictionary<String,String>? getIntencoes(String texto)
    {
    String texto_tratado = texto.ToLower();
    String[] texto_split = texto_tratado.Split(" ");
    Dictionary<String,String>? intencoes_return = new Dictionary<String,String>();
    // String : intenção especifica (pizza, bebida, encerrar pedido etc)
    // String[] : texto da intenção
     
        for(int i=0;i<texto_split.Length;i++) // ver todas as palavras em "texto_split"
        {
        String i_texto = texto_split[i]; // texto i do split
        String? intencao_key_try = intencoes_dicionario.GetValueOrDefault(i_texto); // ver se "i_texto" é uma intenção e caso seja, retorna sua chave 
            if(intencao_key_try != null) // se for percebido uma intenção 
            {
            String intencao_texto = i_texto; // lista com o texto da intenção até o pedido
                for(int j=i+1;j<texto_split.Length;j++) // olhar texto até perceber outra intenção 
                {
                var j_texto = texto_split[j];
                    if(intencoes_dicionario.GetValueOrDefault(j_texto) == null) // se "j_texto" NÃO for uma intenção  
                    {
                    intencao_texto += " " + j_texto;
                    i = j;
                    }
                    else
                    {
                    j = texto_split.Length;
                    }
                }
            String? possivel_intencao = intencoes_return.GetValueOrDefault(intencao_key_try); // ver se essa intenção já existe 
                if(possivel_intencao == null) // se essa intenção não for repitida 
                {
                intencoes_return.Add(intencao_key_try,intencao_texto);         
                }
                else // se essa intenção já existe
                {
                var i_intencao_uptade = possivel_intencao + "|" + intencao_texto;
                intencoes_return.Remove(intencao_key_try);
                intencoes_return.Add(intencao_key_try,i_intencao_uptade);
                }
            }
        }
    if(intencoes_return.Count == 0) return null; 
    return intencoes_return;
    }
}