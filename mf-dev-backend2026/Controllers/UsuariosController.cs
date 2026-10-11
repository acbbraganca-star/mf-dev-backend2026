
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mf_dev_backend2026.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace mf_dev_backend2026.Controllers;

[Authorize (Roles = "Administrador")]
public class UsuariosController : Controller
{
    private readonly AppDbContext _context;

    public UsuariosController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Usuarios
    public async Task<IActionResult> Index()
    {
        return View(await _context.Usuarios.ToListAsync());
    }
   

    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([Bind("Id,Senha")] Usuario usuario)
    {
        
        var dados = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == usuario.Id);

        
        if (dados == null)
        {
            ViewBag.Message = "Código e/ou senha inválidos!";
            return View();
        }

        
        bool senhaOk = BCrypt.Net.BCrypt.Verify(usuario.Senha, dados.Senha);

        if (!senhaOk)
        {
            ViewBag.Message = "Usuário e/ou senha inválidos!";
            return View();
        }

        
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, dados.Nome),
        new Claim(ClaimTypes.NameIdentifier, dados.Id.ToString()),
        new Claim(ClaimTypes.Role, dados.Perfil.ToString())
    };

        
        var identidade = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identidade);

        
        var propriedades = new AuthenticationProperties
        {
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),   // uma jornada de trabalho
            IsPersistent = true
        };

        
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, principal, propriedades);

        return RedirectToAction("Index", "Home");
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Usuarios");
    }

    // GET: Usuarios/AccessDenied  
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }


    // GET: Usuarios/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == id);
        if (usuario == null) return NotFound();

        return View(usuario);
    }

    // GET: Usuarios/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Usuarios/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nome,Senha,Perfil")] Usuario usuario)
    {
        if (ModelState.IsValid)
        {
           
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);

            _context.Add(usuario);
            await _context.SaveChangesAsync();
            TempData["Mensagem"] = $"Usuário criado! Código de acesso: {usuario.Id}";
            return RedirectToAction(nameof(Index));
            
        }
        return View(usuario);
    }

    // GET: Usuarios/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return NotFound();

        usuario.Senha = string.Empty;   
        return View(usuario);
    }

    // POST: Usuarios/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Senha,Perfil")] Usuario usuario)
    {
        if (id != usuario.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
                _context.Update(usuario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(usuario.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(usuario);
    }

    // GET: Usuarios/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == id);
        if (usuario == null) return NotFound();

        return View(usuario);
    }

    // POST: Usuarios/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario != null)
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool UsuarioExists(int id)
    {
        return _context.Usuarios.Any(e => e.Id == id);
    }
}