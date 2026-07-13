# Pedidos360

Aplicacion web **ASP.NET Core MVC (.NET 9)** para la gestion de categorias, productos,
clientes y pedidos, con exportacion de cada pedido a PDF.

---

## 1. Requisitos previos

| Herramienta | Version minima |
|---|---|
| [.NET SDK](https://dotnet.microsoft.com/download) | 9.0 |
| SQL Server (Express, Developer o LocalDB) | 2019+ |
| Visual Studio 2022 (17.12+) **o** VS Code + extension C# | — |
| EF Core CLI (solo si vas a usar `dotnet ef`) | `dotnet tool install --global dotnet-ef` |

---

## 2. Clonar el repositorio

```bash
git clone <https://github.com/RBBK7/Pedidos360.git>
cd Pedidos360
```

---

## 3. Configurar la cadena de conexion

Archivo `appsettings.json`:

```json
"ConnectionStrings": {
  "Pedidos360Db": "Server=localhost\\SQLEXPRESS;Database=Pedidos360;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;"
}
```

Ajusta `Server=` segun tu instancia local de SQL Server (`localhost`, `.\SQLEXPRESS`,
`(localdb)\MSSQLLocalDB`, etc.).

---

## 4. Crear la base de datos

1. Abre SQL Server Management Studio / Azure Data Studio.
2. Ejecuta el script `DB/BD pedidos360.sql` completo (crea la base `Pedidos360`)
3. Verifica que la base de datos `Pedidos360` se creo correctamente.


## 5. Ejecutar el proyecto

**Visual Studio:** abrir `Pedidos360.sln`/`Pedidos360.csproj` y presionar `F5` o `Ctrl+F5`.


La aplicacion queda disponible en:

- `http://localhost:5000`
- `https://localhost:7000`

(puertos definidos en `Properties/launchSettings.json`).

---

## 6. Usuarios de prueba


---

## 8. Orden de revision sugerido

1. **Home** — tarjetas con totales reales: categorias, productos, productos activos y
   pedidos.
2. **Categorias** — CRUD basico.
3. **Productos** — CRUD, carga de imagen, stock (se marca en **rojo** cuando el stock es
   ≤ 1).
4. **Clientes** — CRUD y direccion (provincia/canton/distrito). El campo Estado ya no se
   muestra (aplica solo a Productos y Pedidos); la cedula no se puede editar una vez
   creado el cliente.
5. **Pedidos**
   - Crear un pedido nuevo (autosuggest de productos, calculo de subtotal/descuento/impuesto/total en vivo, descuento de stock al confirmar).
   - En el listado o en el detalle, cambiar el estado con el menu desplegable
     (se guarda al instante por AJAX, sin recargar la pagina).
   - Exportar el detalle del pedido a PDF con el boton **"Exportar PDF"**

---

## 9. Librerias y patrones utilizados

| Libreria / patron | Uso |
|---|---|
| **Entity Framework Core 9 (SQL Server)** | Acceso a datos. |
| **QuestPDF** (licencia Community, gratuita) | Generacion del PDF de detalle de pedido(`Services/PedidoPdfService.cs`). |
| **X.PagedList / X.PagedList.Mvc.Core** | Paginacion de listados (Pedidos). |
| **jQuery + jquery-validation + jquery-validation-unobtrusive** | Ya incluidas en `wwwroot/lib` y cargadas desde `_Layout.cshtml`; validacion de formularios en cliente. |
| **Bootstrap 5** | Estilos e interfaz. |
| **Fetch API (JavaScript)** | Cambio de estado del pedido via AJAX desde el listado y el detalle (`Views/Pedidos/Index.cshtml`, `Details.cshtml`), y endpoints AJAX propios del modulo de Pedidos (`BuscarProductos`, `Calcular`, `Confirmar`). |

---

## 10. Notas tecnicas relevantes

- Al confirmar un pedido se descuenta el stock de cada producto dentro de unatransaccion, y se le asigna el estado "Pendiente" (tabla generica `Estados`,
  compartida con Clientes y Productos).
- El descuento por linea de pedido se maneja como **porcentaje** (0–100 %).


---


