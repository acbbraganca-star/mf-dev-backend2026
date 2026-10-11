
using mf_dev_backend2026.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace mf_dev_backend2026.Controllers;

[Authorize]
public class ConsumoController : Controller
{
    private readonly AppDbContext _context;

    public ConsumoController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Consumo
    public async Task<IActionResult> Index()
    {
        var consumos = _context.Consumos.Include(c => c.Veiculo);
        return View(await consumos.ToListAsync());
    }

    // GET: Consumo/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var consumo = await _context.Consumos
            .Include(c => c.Veiculo)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (consumo == null) return NotFound();

        return View(consumo);
    }

    // GET: Consumo/Create
    public IActionResult Create()
    {
        CarregarVeiculos();
        return View();
    }

    // POST: Consumo/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Descricao,Data,Local,Valor,Quilometragem,TipoCombustivel,VeiculoId")] Consumo consumo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(consumo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        CarregarVeiculos(consumo.VeiculoId);   // recarrega a lista se houver erro
        return View(consumo);
    }

    // GET: Consumo/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var consumo = await _context.Consumos.FindAsync(id);
        if (consumo == null) return NotFound();

        CarregarVeiculos(consumo.VeiculoId);
        return View(consumo);
    }

    // POST: Consumo/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("Id,Descricao,Data,Local,Valor,Quilometragem,TipoCombustivel,VeiculoId")] Consumo consumo)
    {
        if (id != consumo.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(consumo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConsumoExists(consumo.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        CarregarVeiculos(consumo.VeiculoId);
        return View(consumo);
    }

    // GET: Consumo/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var consumo = await _context.Consumos
            .Include(c => c.Veiculo)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (consumo == null) return NotFound();

        return View(consumo);
    }

    // POST: Consumo/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var consumo = await _context.Consumos.FindAsync(id);
        if (consumo != null)
        {
            _context.Consumos.Remove(consumo);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool ConsumoExists(int id)
    {
        return _context.Consumos.Any(e => e.Id == id);
    }

    // Monta a lista do dropdown: valor = Id, texto exibido = Nome
    private void CarregarVeiculos(int? veiculoSelecionado = null)
    {
        ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Nome", veiculoSelecionado);
    }
}