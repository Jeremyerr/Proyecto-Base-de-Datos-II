(function () {
    const bd = db.getSiblingDB("InventarioDB");

    let semilla = 7112026;

    function rnd() {
        semilla = (semilla * 1664525 + 1013904223) % 4294967296;
        return semilla / 4294967296;
    }

    function entero(min, max) {
        return min + Math.floor(rnd() * (max - min + 1));
    }

    function elegir(arr) {
        return arr[Math.floor(rnd() * arr.length)];
    }

    bd.usuarios.deleteMany({ Email: /^usuario\d+@api\.com$/ });
    bd.productos.deleteMany({});
    bd.movimientos.deleteMany({});

    const usuarios = [];

    for (let i = 1; i <= 60; i++) {
        usuarios.push({
            Email: "usuario" + i + "@api.com",
            Password: "Examen123!",
            Rol: "Visualizador"
        });
    }

    bd.usuarios.insertMany(usuarios);

    const proveedores = [
        { Nombre: "Distribuidora Cuscatlán", Contacto: "ventas@cuscatlan.com.sv" },
        { Nombre: "Importadora El Salvador", Contacto: "2222-1010" },
        { Nombre: "Suministros Pacífico", Contacto: "contacto@sumpacifico.com" },
        { Nombre: "Comercial San Miguel", Contacto: "2661-3030" },
        { Nombre: "Tecnoimport S.A.", Contacto: "info@tecnoimport.com.sv" },
        { Nombre: "Papelería La Central", Contacto: "2271-4545" },
        { Nombre: "Ferretería Los Andes", Contacto: "ventas@losandes.com.sv" },
        { Nombre: "Mobiliarios del Valle", Contacto: "2556-7070" }
    ];

    const catalogo = {
        "Papelería": [
            "Resma de papel carta",
            "Cuaderno profesional",
            "Caja de bolígrafos azules",
            "Marcadores permanentes",
            "Folder manila",
            "Grapadora metálica",
            "Cinta adhesiva",
            "Block de notas adhesivas",
            "Tijeras de oficina",
            "Carpeta de anillos",
            "Calculadora de escritorio",
            "Corrector líquido"
        ],
        "Tecnología": [
            "Mouse inalámbrico",
            "Teclado USB",
            "Memoria USB 64GB",
            "Monitor 24 pulgadas",
            "Cable HDMI 2m",
            "Router WiFi",
            "Audífonos con micrófono",
            "Disco duro externo 1TB",
            "Regulador de voltaje",
            "Cámara web HD",
            "Impresora láser",
            "Tóner negro"
        ],
        "Limpieza": [
            "Desinfectante galón",
            "Detergente en polvo",
            "Escoba de cerdas",
            "Trapeador",
            "Bolsas de basura grandes",
            "Jabón líquido para manos",
            "Papel higiénico paquete",
            "Guantes de látex",
            "Cloro galón",
            "Limpiavidrios",
            "Esponjas de cocina",
            "Ambientador"
        ],
        "Herramientas": [
            "Martillo de acero",
            "Juego de destornilladores",
            "Taladro inalámbrico",
            "Cinta métrica 5m",
            "Alicate universal",
            "Llave ajustable",
            "Caja de clavos",
            "Nivel de burbuja",
            "Candado de seguridad",
            "Extensión eléctrica",
            "Sierra manual",
            "Lentes de seguridad"
        ],
        "Mobiliario": [
            "Silla ergonómica",
            "Escritorio de oficina",
            "Archivero metálico",
            "Estante de 4 niveles",
            "Mesa de reuniones",
            "Pizarra blanca",
            "Lámpara de escritorio",
            "Silla de visita",
            "Perchero de pared",
            "Gabinete con llave",
            "Reposapiés",
            "Papelera metálica"
        ]
    };

    const etiquetasPorCategoria = {
        "Papelería": ["oficina", "escolar", "consumible"],
        "Tecnología": ["electrónico", "oficina", "garantía"],
        "Limpieza": ["consumible", "higiene", "hogar"],
        "Herramientas": ["construcción", "manual", "resistente"],
        "Mobiliario": ["oficina", "madera", "metal"]
    };

    const etiquetasExtra = ["oferta", "nuevo", "importado", "más vendido"];

    const productos = [];

    Object.keys(catalogo).forEach(function (categoria) {
        catalogo[categoria].forEach(function (nombre) {
            const etiquetas = [elegir(etiquetasPorCategoria[categoria])];
            const segunda = elegir(etiquetasPorCategoria[categoria]);

            if (etiquetas.indexOf(segunda) === -1) {
                etiquetas.push(segunda);
            }

            if (rnd() < 0.4) {
                etiquetas.push(elegir(etiquetasExtra));
            }

            productos.push({
                Nombre: nombre,
                Descripcion: nombre + " de la categoría " + categoria + ", disponible en bodega.",
                Precio: Double(Math.round((rnd() * 145 + 2) * 100) / 100),
                Stock: NumberInt(entero(0, 150)),
                Categoria: categoria,
                Etiquetas: etiquetas,
                Proveedor: elegir(proveedores)
            });
        });
    });

    const resP = bd.productos.insertMany(productos);

    const productoIds = Object.keys(resP.insertedIds).map(function (k) {
        return resP.insertedIds[k];
    });

    const motivosEntrada = [
        "Compra a proveedor",
        "Devolución de cliente",
        "Ajuste de inventario"
    ];

    const motivosSalida = [
        "Venta",
        "Consumo interno",
        "Merma",
        "Ajuste de inventario"
    ];

    const BASE = new Date("2026-11-07T12:00:00Z").getTime();

    const movimientos = [];

    for (let i = 0; i < 80; i++) {
        const tipo = rnd() < 0.5 ? "Entrada" : "Salida";

        movimientos.push({
            ProductoId: elegir(productoIds),
            Tipo: tipo,
            Cantidad: NumberInt(entero(1, 40)),
            Fecha: new Date(
                BASE -
                entero(0, 180) * 86400000 -
                entero(0, 86399) * 1000
            ),
            Motivo: tipo === "Entrada"
                ? elegir(motivosEntrada)
                : elegir(motivosSalida)
        });
    }

    bd.movimientos.insertMany(movimientos);

    print(
        "Seed listo -> usuarios: " + bd.usuarios.countDocuments() +
        ", productos: " + bd.productos.countDocuments() +
        ", movimientos: " + bd.movimientos.countDocuments()
    );
})();
