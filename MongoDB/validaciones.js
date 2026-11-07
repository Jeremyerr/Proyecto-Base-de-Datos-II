
(function () {
    const bd = db.getSiblingDB("InventarioDB");

    const validaciones = {
        productos: {
            bsonType: "object",
            required: ["Nombre", "Descripcion", "Precio", "Stock", "Proveedor"],
            properties: {
                Nombre: { bsonType: "string", minLength: 1 },
                Descripcion: { bsonType: "string" },
                Precio: { bsonType: "number", minimum: 0 },
                Stock: { bsonType: "number", minimum: 0 },
                Categoria: { bsonType: "string" },
                Etiquetas: {
                    bsonType: "array",
                    items: { bsonType: "string" }
                },
                Proveedor: {
                    bsonType: "object",
                    required: ["Nombre", "Contacto"],
                    properties: {
                        Nombre: { bsonType: "string" },
                        Contacto: { bsonType: "string" }
                    }
                }
            }
        },
        usuarios: {
            bsonType: "object",
            required: ["Email", "Password", "Rol"],
            properties: {
                Email: {
                    bsonType: "string",
                    pattern: "^.+@.+\\..+$"
                },
                Password: { bsonType: "string", minLength: 6 },
                Rol: { enum: ["Creador", "Visualizador"] }
            }
        },
        movimientos: {
            bsonType: "object",
            required: ["ProductoId", "Tipo", "Cantidad", "Fecha", "Motivo"],
            properties: {
                ProductoId: { bsonType: "objectId" },
                Tipo: { enum: ["Entrada", "Salida"] },
                Cantidad: { bsonType: "number", minimum: 1 },
                Fecha: { bsonType: "date" },
                Motivo: { bsonType: "string", minLength: 1 }
            }
        }
    };

    Object.entries(validaciones).forEach(function ([nombre, esquema]) {
        const opciones = {
            validator: { $jsonSchema: esquema },
            validationLevel: "strict",
            validationAction: "error"
        };

        if (bd.getCollectionInfos({ name: nombre }).length === 0) {
            bd.createCollection(nombre, opciones);
        } else {
            const resultado = bd.runCommand({
                collMod: nombre,
                ...opciones
            });

            if (!resultado.ok) {
                throw new Error(
                    "No se pudo validar la colección " +
                    nombre + ": " + resultado.errmsg
                );
            }
        }

        print("Validación configurada: " + nombre);
    });
})();
