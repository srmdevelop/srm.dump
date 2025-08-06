using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Runtime.InteropServices;

namespace dump
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    class Program
    {
        static void Main()
        {
            try
            {
                StringBuilder inventarioCompleto = new StringBuilder();                
                inventarioCompleto.AppendLine($"#iventbia# - {DateTime.Now}");
                inventarioCompleto.AppendLine();
                
                try
                {
                    Console.WriteLine("Passo 1 de 11 - Coletando informações da placa mãe");
                    string resultado = ExecutarComando("cmd.exe", "/c wmic baseboard get product,Manufacturer,version,serialnumber");                    
                    if (!string.IsNullOrWhiteSpace(resultado) && !resultado.Contains("não é reconhecido"))
                    {
                        inventarioCompleto.AppendLine("#placa_mae wmic#");
                        inventarioCompleto.AppendLine(resultado);
                        inventarioCompleto.AppendLine();
                    }
                }
                catch
                {
                    // ignored
                }

                try
                {
                    Console.WriteLine("Passo 2 de 11 - Coletando informações do processador");
                    string resultado = ExecutarComando("cmd.exe", "/c wmic cpu get name,manufacturer,maxclockspeed,numberofcores,numberoflogicalprocessors");
                    if (!string.IsNullOrWhiteSpace(resultado) && !resultado.Contains("não é reconhecido"))
                    {
                        inventarioCompleto.AppendLine("#processador wmic#");
                        inventarioCompleto.AppendLine(resultado);
                        inventarioCompleto.AppendLine();
                    }
                }
                catch
                {
                    // ignored
                }

                try
                {
                    Console.WriteLine("Passo 3 de 11 - Coletando informações da memória");
                    string resultado = ExecutarComando("cmd.exe", "/c wmic memorychip get capacity,speed,manufacturer");
                    if (!string.IsNullOrWhiteSpace(resultado) && !resultado.Contains("não é reconhecido"))
                    {
                        inventarioCompleto.AppendLine("#memoria wmic#");
                        inventarioCompleto.AppendLine(resultado);
                        inventarioCompleto.AppendLine();
                    }
                }
                catch
                {
                    // ignored
                }

                try
                {
                    Console.WriteLine("Passo 4 de 11 - Coletando informações do disco");
                    string resultado = ExecutarComando("cmd.exe", "/c wmic diskdrive get model,size,interfacetype,serialnumber");
                    if (!string.IsNullOrWhiteSpace(resultado) && !resultado.Contains("não é reconhecido"))
                    {
                        inventarioCompleto.AppendLine("#disco wmic#");
                        inventarioCompleto.AppendLine(resultado);
                        inventarioCompleto.AppendLine();
                    }
                }
                catch
                {
                    // ignored
                }

                try
                {
                    Console.WriteLine("Passo 5 de 11 - Coletando informações da placa mãe");
                    string resultado = ExecutarComando("powershell.exe", "-Command \"Get-CimInstance -ClassName Win32_BaseBoard | Format-List *\"");
                    if (!string.IsNullOrWhiteSpace(resultado))
                    {
                        inventarioCompleto.AppendLine("#placa_mae powershell#");
                        inventarioCompleto.AppendLine(resultado);
                        inventarioCompleto.AppendLine();
                    }
                }
                catch
                {
                    // ignored
                }

                try
                {
                    Console.WriteLine("Passo 6 de 11 - Coletando informações do BIOS");
                    string resultado = ExecutarComando("powershell.exe", "-Command \"Get-CimInstance -ClassName Win32_BIOS | Format-List *\"");
                    if (!string.IsNullOrWhiteSpace(resultado))
                    {
                        inventarioCompleto.AppendLine("#bios powershell#");
                        inventarioCompleto.AppendLine(resultado);
                        inventarioCompleto.AppendLine();
                    }
                }
                catch
                {
                    // ignored
                }

                try
                {
                    Console.WriteLine("Passo 7 de 11 - Coletando informações do sistema");
                    string resultado = ExecutarComando("cmd.exe", "/c systeminfo");
                    if (!string.IsNullOrWhiteSpace(resultado))
                    {
                        inventarioCompleto.AppendLine("#sistema systeminfo#");
                        inventarioCompleto.AppendLine(resultado);
                        inventarioCompleto.AppendLine();
                    }
                }
                catch
                {
                    // ignored
                }

                try
                {
                    Console.WriteLine("Passo 8 de 11 - Coletando informações da rede");
                    string resultado = ExecutarComando("cmd.exe", "/c ipconfig /all");
                    if (!string.IsNullOrWhiteSpace(resultado))
                    {
                        inventarioCompleto.AppendLine("#rede ipconfig#");
                        inventarioCompleto.AppendLine(resultado);
                        inventarioCompleto.AppendLine();
                    }
                }
                catch
                {
                    // ignored
                }

                try
                {
                    Console.WriteLine("Passo 9 de 11 - Coletando informações do sistema");
                    string resultado = ObterInformacoesBasicas();
                    if (!string.IsNullOrWhiteSpace(resultado))
                    {
                        inventarioCompleto.AppendLine("#sistema .net#");
                        inventarioCompleto.AppendLine(resultado);
                        inventarioCompleto.AppendLine();
                    }
                }
                catch
                {
                    // ignored
                }

                try
                {
                    Console.WriteLine("Passo 10 de 11 - Coletando informações dos drivers");
                    string resultado = ExecutarComando("cmd.exe", "/c driverquery /v");
                    if (!string.IsNullOrWhiteSpace(resultado))
                    {
                        inventarioCompleto.AppendLine("#drivers driverquery#");
                        inventarioCompleto.AppendLine(resultado);
                        inventarioCompleto.AppendLine();
                    }
                }
                catch
                {
                    // ignored
                }

                // Aplicações instaladas via WMIC
                try
                {
                    Console.WriteLine("Passo 11 de 11 - Coletando informações das aplicações instaladas");
                        string resultado = ExecutarComando("cmd.exe", "/c wmic product get name,version,vendor");
                    if (!string.IsNullOrWhiteSpace(resultado) && !resultado.Contains("não é reconhecido"))
                    {
                        inventarioCompleto.AppendLine("#aplicacoes wmic#");
                        inventarioCompleto.AppendLine(resultado);
                        inventarioCompleto.AppendLine();
                    }
                }
                catch
                {
                    // ignored
                }

                // Aplicações instaladas via PowerShell
                try
                {
                    string resultado = ExecutarComando("powershell.exe", "-Command \"Get-ItemProperty HKLM:\\Software\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\* | Select-Object DisplayName, DisplayVersion, Publisher | Format-Table -AutoSize\"");
                    if (!string.IsNullOrWhiteSpace(resultado))
                    {
                        inventarioCompleto.AppendLine("#aplicacoes powershell 32bits#");
                        inventarioCompleto.AppendLine(resultado);
                        inventarioCompleto.AppendLine();
                    }
                }
                catch
                {
                    // ignored
                }

                // Aplicações instaladas via PowerShell (64 bits)
                try
                {
                    string resultado = ExecutarComando("powershell.exe", "-Command \"Get-ItemProperty HKLM:\\Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\* | Select-Object DisplayName, DisplayVersion, Publisher | Format-Table -AutoSize\"");
                    if (!string.IsNullOrWhiteSpace(resultado))
                    {
                        inventarioCompleto.AppendLine("#aplicacoes powershell 64bits#");
                        inventarioCompleto.AppendLine(resultado);
                        inventarioCompleto.AppendLine();
                    }
                }
                catch
                {
                    // ignored
                }

                // Lista de usuários locais via comando net user
                try
                {
                    string resultado = ExecutarComando("cmd.exe", "/c net user");
                    if (!string.IsNullOrWhiteSpace(resultado))
                    {
                        inventarioCompleto.AppendLine("#usuarios locais#");
                        inventarioCompleto.AppendLine(resultado);
                        inventarioCompleto.AppendLine();
                    }
                }
                catch
                {
                    // ignored
                }

                // Lista de usuários via PowerShell (mais detalhado)
                try
                {
                    string resultado = ExecutarComando("powershell.exe", "-Command \"Get-LocalUser | Select-Object Name, Enabled, LastLogon, PasswordRequired, PasswordLastSet | Format-Table -AutoSize\"");
                    if (!string.IsNullOrWhiteSpace(resultado))
                    {
                        inventarioCompleto.AppendLine("#usuarios powershell#");
                        inventarioCompleto.AppendLine(resultado);
                        inventarioCompleto.AppendLine();
                    }
                }
                catch
                {
                    // ignored
                }

                // Lista de grupos locais
                try
                {
                    string resultado = ExecutarComando("cmd.exe", "/c net localgroup");
                    if (!string.IsNullOrWhiteSpace(resultado))
                    {
                        inventarioCompleto.AppendLine("#grupos locais#");
                        inventarioCompleto.AppendLine(resultado);
                        inventarioCompleto.AppendLine();
                    }
                }
                catch
                {
                    // ignored
                }

                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "inventario.txt");
                // Criar o novo arquivo de inventário
                File.WriteAllText(filePath, inventarioCompleto.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                try
                {
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "inventario.txt");
                    File.WriteAllText(filePath, $"Erro ao coletar informações: {ex.Message}", Encoding.UTF8);
                }
                catch
                {
                    // ignored
                }
            }
        }
        
        private static string ExecutarComando(string programa, string argumentos)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = programa;
                process.StartInfo.Arguments = argumentos;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                
                return output;
            }
        }
        
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetPhysicallyInstalledSystemMemory(out long TotalMemoryInKilobytes);
        
        private static string ObterInformacoesBasicas()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.AppendLine($"Sistema Operacional: {Environment.OSVersion}");
            sb.AppendLine($"Nome da Máquina: {Environment.MachineName}");
            sb.AppendLine($"Processadores: {Environment.ProcessorCount}");
            
            if (GetPhysicallyInstalledSystemMemory(out long memoryInKb))
            {
                sb.AppendLine($"Memória Física: {memoryInKb / (1024 * 1024)} GB");
            }
            
            return sb.ToString();
        }
    }
}