﻿using System;
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
            System.Console.Write("Digite o código do tipo de envio: ");
            var nCdServico = System.Console.ReadLine();
            System.Console.Write("Digite o CEP de origem: ");
            var sCepOrigem = System.Console.ReadLine();
            System.Console.Write("Digite o CEP de destino: ");
            var sCepDestino = System.Console.ReadLine();
            System.Console.Write("Digite o peso (kg): ");

            var nVlPeso = System.Console.ReadLine();
            while (nVlPeso.Contains(","))
            {
                System.Console.WriteLine("Utilize ponto(.) para separador de casas decimais");
                System.Console.Write("Digite o peso (kg): ");
                nVlPeso = System.Console.ReadLine();
            }

            System.Console.Write("Digite o código do formato (caixa, envelope, etc): ");
            var nCdFormato = System.Console.ReadLine();

            while (nCdFormato.Contains(",") || nCdFormato.Contains("."))
            {
                System.Console.WriteLine("O Código informado é inválido, digite 1 para Caixa / Pacote, 2 para Rolo / Prisma ou 3 para envelop");
                System.Console.Write("Digite o código do formato (caixa, envelope, etc): ");
                nCdFormato = System.Console.ReadLine();                
            }          
                
        

            System.Console.Write("Digite o comprimento: ");
            var nVlComprimento = System.Console.ReadLine();
            System.Console.Write("Digite a altura: ");
            var nVlAltura = System.Console.ReadLine();
            System.Console.Write("Digite a largura: ");
            var nVlLargura = System.Console.ReadLine();
            System.Console.Write("Digite o diâmetro: ");
            var nVlDiametro = System.Console.ReadLine();
            System.Console.Write("Entrega em mão própria (S/N)?: ");
            var sCdMaoPropria = System.Console.ReadLine();
            System.Console.Write("Digite o valor declarado: ");
            var nVlValorDeclarado = System.Console.ReadLine();
            System.Console.Write("Aviso de recebimento (S/N)?: ");
            var sCdAvisoRecebimento = System.Console.ReadLine();


            string nCdEmpresa = string.Empty;
            string sDsSenha = string.Empty;            

            try
            {

                WSCorreiosCalculaPreco.CalcPrecoPrazoWSSoapClient ws = new WSCorreiosCalculaPreco.CalcPrecoPrazoWSSoapClient();

                var resposta = ws.CalcPrecoPrazo(nCdEmpresa,sDsSenha,nCdServico,
                    sCepOrigem,sCepDestino,nVlPeso, int.Parse(nCdFormato),decimal.Parse(nVlComprimento),
                    decimal.Parse(nVlAltura),decimal.Parse(nVlLargura),decimal.Parse(nVlDiametro),sCdMaoPropria.ToUpper(),decimal.Parse(nVlValorDeclarado),sCdAvisoRecebimento.ToUpper());
                var respostaReal = resposta.Servicos.FirstOrDefault();
                if (respostaReal != null)
                {
                    System.Console.WriteLine("\n");
                    System.Console.WriteLine("Entrega em mãos: {0}", (respostaReal.EntregaDomiciliar.Equals("N") ? "Não" : "Sim"));
                    System.Console.WriteLine("Prazo: {0} dia(s)", respostaReal.PrazoEntrega);
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
