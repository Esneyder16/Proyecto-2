using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    // Pilas para clasificación de pacientes (Pilas)
    private static Stack<Paciente> urgencias = new Stack<Paciente>();
    private static Stack<Paciente> prioridadNormal = new Stack<Paciente>();
    private static Stack<Paciente> bajaPrioridad = new Stack<Paciente>();

    // Lista para almacenar todos los pacientes registrados (Lista)
    private static List<Paciente> listaPacientes = new List<Paciente>();

    // Estadísticas
    private static Dictionary<string, int> pacientesAtendidosPorGravedad = new Dictionary<string, int>
    {
        { "Urgencia", 0 },
        { "Prioridad Normal", 0 },
        { "Baja Prioridad", 0 }
    };

    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Sistema de Atención en Hospital");
            Console.WriteLine("1. Agregar Paciente");
            Console.WriteLine("2. Atender Paciente");
            Console.WriteLine("3. Mostrar Pacientes en Espera");
            Console.WriteLine("4. Mostrar Estadísticas");
            Console.WriteLine("5. Salir");
            Console.Write("Selecciona una opción: ");
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    AgregarPaciente();
                    break;
                case "2":
                    AtenderPaciente();
                    break;
                case "3":
                    MostrarPacientesEnEspera();
                    break;
                case "4":
                    MostrarEstadisticas();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Opción no válida. Inténtalo de nuevo.");
                    break;
            }
        }
    }

    private static void AgregarPaciente()
    {
        Console.Write("Nombre del paciente: ");
        string nombre = Console.ReadLine();
        Console.Write("Cédula del paciente: ");
        string cedula = Console.ReadLine();

        // La hora de llegada se establece en la hora actual
        DateTime horaLlegada = DateTime.Now;

        Console.WriteLine("Nivel de gravedad:");
        Console.WriteLine("1. Urgencia");
        Console.WriteLine("2. Prioridad Normal");
        Console.WriteLine("3. Baja Prioridad");
        string nivel = Console.ReadLine();

        Paciente paciente = new Paciente
        {
            Nombre = nombre,
            Cedula = cedula,
            HoraLlegada = horaLlegada, // Usar la hora actual
            NivelGravedad = nivel
        };

        // Clasificación de pacientes según el nivel de gravedad (Pilas)
        switch (nivel)
        {
            case "1":
                urgencias.Push(paciente); // Añadir paciente a la pila de urgencias
                break;
            case "2":
                prioridadNormal.Push(paciente); // Añadir paciente a la pila de prioridad normal
                break;
            case "3":
                bajaPrioridad.Push(paciente); // Añadir paciente a la pila de baja prioridad
                break;
            default:
                Console.WriteLine("Nivel de gravedad no válido.");
                return;
        }

        // Añadir paciente a la lista de todos los pacientes (Lista)
        listaPacientes.Add(paciente);

        Console.WriteLine("Paciente agregado exitosamente.");
        Console.ReadKey();
    }

    private static void AtenderPaciente()
    {
        // Atender a los pacientes en el siguiente orden de gravedad:
        // 1. Urgencias
        // 2. Prioridad Normal
        // 3. Baja Prioridad

        if (urgencias.Count > 0) // Primero atender pacientes de urgencias
        {
            AtenderPacienteDePila(urgencias, "Urgencia");
        }
        else if (prioridadNormal.Count > 0) // Luego, atender pacientes de prioridad normal
        {
            AtenderPacienteDePila(prioridadNormal, "Prioridad Normal");
        }
        else if (bajaPrioridad.Count > 0) // Finalmente, atender pacientes de baja prioridad
        {
            AtenderPacienteDePila(bajaPrioridad, "Baja Prioridad");
        }
        else
        {
            Console.WriteLine("No hay pacientes en espera.");
        }

        Console.ReadKey();
    }

    private static void AtenderPacienteDePila(Stack<Paciente> pila, string nivelGravedad)
    {
        if (pila.Count > 0) // Comprobar si hay pacientes en la pila
        {
            Paciente paciente = pila.Pop(); // Atender al paciente de la pila
            TimeSpan tiempoEspera = DateTime.Now - paciente.HoraLlegada;

            // Incrementar el conteo de pacientes atendidos por nivel de gravedad
            pacientesAtendidosPorGravedad[nivelGravedad]++;

            Console.WriteLine($"Paciente {paciente.Nombre} atendido. Nivel de gravedad: {nivelGravedad}. Tiempo de espera: {tiempoEspera.TotalMinutes} minutos.");
        }
    }

    private static void MostrarPacientesEnEspera()
    {
        Console.WriteLine("Pacientes en espera:");
        Console.WriteLine("Urgencias:");
        foreach (var paciente in urgencias) // Mostrar pacientes en la pila de urgencias
        {
            Console.WriteLine($"{paciente.Nombre} - {paciente.Cedula} - Hora de llegada: {paciente.HoraLlegada}");
        }

        Console.WriteLine("Prioridad Normal:");
        foreach (var paciente in prioridadNormal) // Mostrar pacientes en la pila de prioridad normal
        {
            Console.WriteLine($"{paciente.Nombre} - {paciente.Cedula} - Hora de llegada: {paciente.HoraLlegada}");
        }

        Console.WriteLine("Baja Prioridad:");
        foreach (var paciente in bajaPrioridad) // Mostrar pacientes en la pila de baja prioridad
        {
            Console.WriteLine($"{paciente.Nombre} - {paciente.Cedula} - Hora de llegada: {paciente.HoraLlegada}");
        }

        Console.WriteLine("Lista completa de pacientes registrados:");
        foreach (var paciente in listaPacientes) // Mostrar todos los pacientes registrados en la lista
        {
            Console.WriteLine($"{paciente.Nombre} - {paciente.Cedula} - Hora de llegada: {paciente.HoraLlegada}");
        }
        Console.ReadKey();
    }

    private static void MostrarEstadisticas()
    {
        double tiempoTotalEspera = 0;
        int cantidadPacientes = 0;

        // Calcular el tiempo de espera promedio
        foreach (var paciente in listaPacientes)
        {
            TimeSpan tiempoEspera = DateTime.Now - paciente.HoraLlegada;
            tiempoTotalEspera += tiempoEspera.TotalMinutes;
            cantidadPacientes++;
        }

        double tiempoPromedioEspera = cantidadPacientes > 0 ? tiempoTotalEspera / cantidadPacientes : 0;

        Console.WriteLine($"Tiempo promedio de espera: {tiempoPromedioEspera:F2} minutos.");

        Console.WriteLine("Cantidad de pacientes atendidos por nivel de gravedad:");
        foreach (var gravedad in pacientesAtendidosPorGravedad)
        {
            Console.WriteLine($"{gravedad.Key}: {gravedad.Value}");
        }

        Console.ReadKey();
    }
}





