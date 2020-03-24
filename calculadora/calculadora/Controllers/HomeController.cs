using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using calculadora.Models;

namespace Calculadora.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// apresentação inicial da view no browser
        /// </summary>
        /// <returns></returns>

        public IActionResult Index()
        {
            // inicializar o valor do Visor
            ViewBag.Visor = "0";

            return View();
        }

        /// <summary>
        /// apresentação da 'vista' da calculadora, qd a interação é efetuada em modo 'HTTP POST'
        /// </summary>
        /// <param name="visor">representação do operando a utilizar na operação, bem como a resposta após a execução da operação</param>
        /// <param name="bt">valor do botão que foi premido</param>
        /// <param name="operando">vaalor do primerio operando a usar na operação</param>
        /// <param name="operador">símbolo da operação</param>
        /// <param name="limpaVisor">identificação se o visor deve ser limpo, ou não</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Index(string visor, string bt, string operando, string operador, bool limpaVisor)
        {
            //selecionar o valor do 'bt' e atuar em conformidade com o seu significado
            switch (bt)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "0":
                    if (visor == "0" || limpaVisor) visor = bt;
                    else visor += bt;
                    //marcar o visor para NÃO ser limpo
                    limpaVisor = false;
                    break;

                case "+/-":
                    visor = Convert.ToDouble(visor) * -1 + "";
                    //como o 'visor' é uma string, a inversão poderia ser feita por manipulação de strings
                    //precisaríamos dos métodos .StartsWith(), .ToString(), .Substring(), .Length
                    break;

                case ",":
                    if (!visor.Contains(",")) visor += bt;
                    break;

                case "+":
                case "-":
                case ":":
                case "x":
                case "=":

                    //como guardar os dados do 'visor' para ter 'memória'?
                    //é a primeira vez que escolho um operador?
                    if (operador != null)
                    {
                        //carreguei + do que uma vez num 'operando'
                        double primeiroOperando = Convert.ToDouble(operando);
                        double segundoOperando = Convert.ToDouble(visor);
                        switch (operador)
                        {
                            case "+":
                                visor = primeiroOperando + segundoOperando + "";
                                break;
                            case "-":
                                visor = primeiroOperando - segundoOperando + "";
                                break;
                            case ":":
                                visor = primeiroOperando / segundoOperando + "";
                                break;
                            case "x":
                                visor = primeiroOperando * segundoOperando + "";
                                break;
                        }

                    }
                    //garantir o efeito de 'memória'
                    if (!bt.Equals("="))
                    {
                        operando = visor;
                        operador = bt;
                    }
                    else
                    {
                        //anula o efeito da 'memória'
                        operando = null;
                        operador = null;
                    }
                    //como dar ordem ao visor para se reiniciar
                    limpaVisor = true;
                    break;

                case "C":
                    visor = "0";
                    operando = null;
                    operador = null;
                    limpaVisor = true;
                    break;

            }

            //enviar dados do Visor para a view
            ViewBag.Visor = visor;
            //e os outros dados, também
            ViewBag.Operador = operador;
            ViewBag.Operando = operando;
            ViewBag.LimpaVisor = limpaVisor + "";



            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}