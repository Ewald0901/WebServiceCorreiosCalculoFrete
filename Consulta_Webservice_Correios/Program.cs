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
        private static bool validarCep(string cep)
        {
            string _cep = cep.Trim(); // remove espaços em branco
            _cep = _cep.Replace("-", ""); // substitui o traço 

            if (_cep.Length == 8)// verifica se o tamanho é 8
                return true;

            else
                return false;
        }

        public static void CalcularPrecoPrazo()
        {
            System.Console.Write("Digite o código do tipo de envio: ");
            var nCdServico = System.Console.ReadLine();

            System.Console.Write("Digite o CEP de origem: ");
            var sCepOrigem = System.Console.ReadLine();           
            while(!validarCep(sCepOrigem))
            {
                System.Console.WriteLine("Cep de Origem Digitado inválido. ");
                System.Console.Write("Digite o CEP de origem novamente: ");
                sCepOrigem = System.Console.ReadLine();
            }

            System.Console.Write("Digite o CEP de destino: ");
            var sCepDestino = System.Console.ReadLine();           
            while (!validarCep(sCepDestino))
            {
                System.Console.WriteLine("Cep de Destino Digitado inválido. ");
                System.Console.Write("Digite o CEP de destino novamente: ");
                sCepOrigem = System.Console.ReadLine();
            }

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

            while (nCdFormato.Contains(",") || nCdFormato.Contains(".") || string.IsNullOrEmpty(nCdFormato))
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
                WSCorreiosCalculaPreco.CalcPrecoPrazoWSSoapClient ws = new WSCorreiosCalculaPreco.CalcPrecoPrazoWSSoapClient(); // cria uma nova instência do serviço do cliente dos correios


                var versao = ws.getVersao();


                // consome o WS para Calcular preço e prazo
                var resposta = ws.CalcPrecoPrazo(nCdEmpresa,sDsSenha,nCdServico,
                    sCepOrigem,sCepDestino,nVlPeso, int.Parse(nCdFormato),decimal.Parse(nVlComprimento),
                    decimal.Parse(nVlAltura),decimal.Parse(nVlLargura),decimal.Parse(nVlDiametro),sCdMaoPropria.ToUpper(),decimal.Parse(nVlValorDeclarado),sCdAvisoRecebimento.ToUpper());
                var respostaReal = resposta.Servicos.FirstOrDefault();
                if (respostaReal != null)
                {
                    System.Console.WriteLine("\n");
                    System.Console.WriteLine("Valor Aviso Recebimento: {0}", (respostaReal.ValorAvisoRecebimento));
                    System.Console.WriteLine("Valor Entrega em mãos: {0}", (respostaReal.ValorMaoPropria));
                    System.Console.WriteLine("Valor sobre o Valor Declarado: {0}", (respostaReal.ValorValorDeclarado));
                    System.Console.WriteLine("Prazo estimado: {0} dia(s)", respostaReal.PrazoEntrega);
                    System.Console.WriteLine("Valor Total: R$ {0}", respostaReal.Valor);
                    System.Console.WriteLine("\n \n");
                    System.Console.WriteLine("Versão do Serviço dos correios {0}", versao.versaoAtual);

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
