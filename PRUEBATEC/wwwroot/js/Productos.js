document.addEventListener("DOMContentLoaded", function () {
    const productos = Array.from(productosData);
    const registrosPorPagina = 5;
    let paginaActual = 1;
    let productosFiltrados = [...productos];
    let columnaOrden = null;
    let ordenAsc = true;

    const tbody = document.getElementById("tbodyProductos");
    const busquedaInput = document.getElementById("busqueda");
    const paginacion = document.getElementById("paginacion");

    function renderTabla() {
        tbody.innerHTML = "";

        const inicio = (paginaActual - 1) * registrosPorPagina;
        const fin = inicio + registrosPorPagina;
        const paginaProductos = productosFiltrados.slice(inicio, fin);

        paginaProductos.forEach(p => {
            const tr = document.createElement("tr");
            tr.innerHTML = `
                <td>${p.NombreP}</td>
                <td>${p.SKU}</td>
                <td>${p.Cantidad}</td>
                <td>${p.PrecioV.toLocaleString('es-MX', { style: 'currency', currency: 'MXN' })}</td>
                <td>${p.PrecioC.toLocaleString('es-MX', { style: 'currency', currency: 'MXN' })}</td>
                <td>${new Date(p.FechaModificacion).toLocaleString()}</td>
                <td>
                    <a class="btn btn-sm btn-warning" href="/Productos/Edit/${p.Id}">Editar</a>
                    <button class="btn btn-sm btn-danger btnEliminar" data-id="${p.Id}">Eliminar</button>
                </td>
            `;
            tbody.appendChild(tr);
        });

        renderPaginacion();
        agregarEventosEliminar();
        actualizarIconos();
    }

    function renderPaginacion() {
        const totalPaginas = Math.ceil(productosFiltrados.length / registrosPorPagina);
        paginacion.innerHTML = "";

        for (let i = 1; i <= totalPaginas; i++) {
            const li = document.createElement("li");
            li.className = "page-item" + (i === paginaActual ? " active" : "");
            const a = document.createElement("a");
            a.className = "page-link";
            a.href = "#";
            a.textContent = i;
            a.addEventListener("click", (e) => {
                e.preventDefault();
                paginaActual = i;
                renderTabla();
            });
            li.appendChild(a);
            paginacion.appendChild(li);
        }
    }

    busquedaInput.addEventListener("input", function () {
        const texto = this.value.toLowerCase();
        productosFiltrados = productos.filter(p =>
            (p.NombreP && p.NombreP.toLowerCase().includes(texto)) ||
            (p.SKU && p.SKU.toLowerCase().includes(texto)) ||
            (p.Cantidad !== null && p.Cantidad.toString().includes(texto)) ||
            (p.PrecioV !== null && p.PrecioV.toString().includes(texto)) ||
            (p.PrecioC !== null && p.PrecioC.toString().includes(texto))
        );
        paginaActual = 1;
        renderTabla();
    });

    document.querySelectorAll(".sortable").forEach(th => {
        th.style.cursor = "pointer";
        const span = document.createElement("span");
        span.className = "sort-icon";
        span.style.marginLeft = "5px";
        span.textContent = " ↕";
        th.appendChild(span);

        th.addEventListener("click", function () {
            const columna = th.dataset.column;
            if (columnaOrden === columna) ordenAsc = !ordenAsc;
            else { columnaOrden = columna; ordenAsc = true; }

            productosFiltrados.sort((a, b) => {
                let valA = a[columna];
                let valB = b[columna];

                // Convertir a número si es posible
                const numA = parseFloat(valA);
                const numB = parseFloat(valB);
                const ambosNumeros = !isNaN(numA) && !isNaN(numB);

                if (ambosNumeros) {
                    return ordenAsc ? numA - numB : numB - numA;
                } else {
                    if (typeof valA === "string") valA = valA.toLowerCase();
                    if (typeof valB === "string") valB = valB.toLowerCase();
                    return ordenAsc ? (valA > valB ? 1 : -1) : (valA < valB ? 1 : -1);
                }
            });

            renderTabla();
        });
    });

    function actualizarIconos() {
        document.querySelectorAll(".sortable").forEach(th => {
            const span = th.querySelector(".sort-icon");
            if (th.dataset.column === columnaOrden) {
                span.textContent = ordenAsc ? " ▲" : " ▼";
            } else {
                span.textContent = " ↕";
            }
        });
    }

    function agregarEventosEliminar() {
        document.querySelectorAll(".btnEliminar").forEach(btn => {
            btn.addEventListener("click", function () {
                const id = this.dataset.id;
                Swal.fire({
                    title: '¿Eliminar producto?',
                    text: "Esta acción no se puede deshacer.",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Sí, eliminar'
                }).then(result => {
                    if (result.isConfirmed) {
                        const form = document.createElement('form');
                        form.method = 'post';
                        form.action = '/Productos/DeleteConfirmed/' + id;

                        const token = document.createElement('input');
                        token.type = 'hidden';
                        token.name = '__RequestVerificationToken';
                        token.value = document.querySelector('input[name="__RequestVerificationToken"]').value;
                        form.appendChild(token);

                        document.body.appendChild(form);
                        form.submit();
                    }
                });
            });
        });
    }

    renderTabla();

    if (typeof alerta !== 'undefined' && alerta && tipoAlerta) {
        Swal.fire({
            icon: tipoAlerta === 'success' ? 'success' : tipoAlerta === 'warning' ? 'warning' : 'error',
            title: tipoAlerta === 'success' ? 'Éxito' : tipoAlerta === 'warning' ? 'Atención' : 'Error',
            text: alerta,
            confirmButtonColor: tipoAlerta === 'success' ? '#3085d6' : tipoAlerta === 'warning' ? '#f0ad4e' : '#d33'
        });
    }
});
