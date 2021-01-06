using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulta_Webservice_Correios
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CalcularPrecoPrazo();
        }

        public static void CalcularPrecoPrazo()
        {
            string nCdEmpresa = string.Empty;
            string sDsSenha = string.Empty;
            string nCdServico = "04510";
            string sCepOrigem = "39860000";
            string sCepDestino = "29101786";
            string nVlPeso = "0.020";
            int nCdFormato = 3;
            decimal nVlComprimento = 15;
            decimal nVlAltura = 0;
            decimal nVlLargura = 10;
            decimal nVlDiametro = 0;
            string sCdMaoPropria = "N";
            decimal nVlValorDeclarado = 0;
            string sCdAvisoRecebimento = "N";

            try
            {


                var ws = new WSCorreiosCalculaPreco.CalcPrecoPrazoWSSoapClient();

                var resposta = ws.CalcPrecoPrazo(nCdEmpresa,sDsSenha,nCdServico,
                    sCepOrigem,sCepDestino,nVlPeso, nCdFormato,nVlComprimento,
                    nVlAltura,nVlLargura,nVlDiametro,sCdMaoPropria,nVlValorDeclarado,sCdAvisoRecebimento);
                var respostaReal = resposta.Servicos.FirstOrDefault();
                if (respostaReal != null)
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine("Prazo: {0} dia", respostaReal.PrazoEntrega);
                    System.Console.WriteLine("Valor: R$ {0}", respostaReal.Valor);
                }
                else
                {
                    throw new Exception("Não foi possível encontrar os valores dentro da resposta do serviço");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Erro ao efetuar cálculos: {0}", ex.Message);
            }
            System.Console.ReadLine();


        }


    }
}
