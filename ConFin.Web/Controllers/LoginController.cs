﻿using ConFin.Application.AppService.Login;
using ConFin.Application.AppService.Usuario;
using ConFin.Common.Domain.Dto;
using ConFin.Common.Web;
using ConFin.Web.ViewModel;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;

namespace ConFin.Web.Controllers
{
    public class LoginController : BaseHomeController
    {
        private readonly ILoginAppService _loginAppService;
        private readonly IUsuarioAppService _usuarioAppService;

        public LoginController(ILoginAppService loginAppService, IUsuarioAppService usuarioAppService)
        {
            _loginAppService = loginAppService;
            _usuarioAppService = usuarioAppService;
        }

        public ActionResult Login()
        {
            try
            {
                return View("Login");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult Get(UsuarioViewModel usuario)
        {
            try
            {
                var response = _loginAppService.Get(usuario.Email, usuario.Senha);
                if (!response.IsSuccessStatusCode)
                    return Error(response);

                UsuarioLogado = JsonConvert.DeserializeObject<UsuarioDto>(response.Content.ReadAsStringAsync().Result);
                return Ok("Login realizado com sucesso.");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult RedirectToHome()
        {
            try
            {
                return RedirectToAction("Home", "Home");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult Post(UsuarioDto usuario)
        {
            try
            {
                var response = _loginAppService.Post(usuario);
                return !response.IsSuccessStatusCode 
                    ? Error(response) 
                    : Ok("Solicitação realizada com sucesso, foi enviado um e-mail para a confirmação do cadastro.");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult GetConfirmacao(int idUsuario)
        {
            try
            {
                var response = _loginAppService.PutConfirmacaoCadastro(idUsuario);
                if (!response.IsSuccessStatusCode)
                    return View("Error", model: response.Content.ReadAsStringAsync().Result);

                return RedirectToAction("Home", "Home");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult PostReenviarSenha(string email)
        {
            try
            {
                var response = _loginAppService.PostReenviarSenha(email);
                return response.IsSuccessStatusCode ? Ok("") : Error(response);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult RedefinirSenha(int idUsuario, string token)
        {
            try
            {
                var response = _loginAppService.GetVerificaTokenValidoRedefinirSenha(idUsuario, token);
                if(!response.IsSuccessStatusCode)
                    return View("Error", model: response.Content.ReadAsStringAsync().Result);

                response = _usuarioAppService.Get(idUsuario);
                if (!response.IsSuccessStatusCode)
                    return View("Error", model: response.Content.ReadAsStringAsync().Result);


                var usuario = JsonConvert.DeserializeObject<UsuarioDto>(response.Content.ReadAsStringAsync().Result);
                ViewBag.Token = token;
                return View("RedefinirSenha", usuario);

            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult PostRedefinirSenha(UsuarioDto usuario, string token)
        {
            try
            {
                var response = _usuarioAppService.PutSenha(usuario.Id, token, usuario.Senha);
                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("Erro", response.Content.ReadAsStringAsync().Result.Replace('[', ' ').Replace(']', ' ').Replace('"', ' '));
                    ViewBag.Token = token;
                    return View("RedefinirSenha", usuario);
                }

                UsuarioLogado = usuario;
                return RedirectToAction("Home", "Home");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

    }
}
