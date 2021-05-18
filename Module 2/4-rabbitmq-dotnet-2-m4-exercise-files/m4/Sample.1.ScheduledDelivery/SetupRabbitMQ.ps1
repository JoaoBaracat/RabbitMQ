param([String]$RabbitDllPath = "not specified")

$RabbitDllPath = Resolve-Path $RabbitDllPath 
Write-Host "Rabbit DLL Path: " 
Write-Host $RabbitDllPath -foregroundcolor green

set-ExecutionPolicy Unrestricted

$absoluteRabbitDllPath = Resolve-Path $RabbitDllPath

Write-Host "Absolute Rabbit DLL Path: " 
Write-Host $absoluteRabbitDllPath -foregroundcolor green

[Reflection.Assembly]::LoadFile($absoluteRabbitDllPath)

Write-Host "Setting up RabbitMQ Connection Factory" -foregroundcolor green
$factory = new-object RabbitMQ.Client.ConnectionFactory
$hostNameProp = [RabbitMQ.Client.ConnectionFactory].GetField(“HostName”)
$hostNameProp.SetValue($factory, “localhost”)

$usernameProp = [RabbitMQ.Client.ConnectionFactory].GetField(“UserName”)
$usernameProp.SetValue($factory, “guest”)

$passwordProp = [RabbitMQ.Client.ConnectionFactory].GetField(“Password”)
$passwordProp.SetValue($factory, “guest”)

$createConnectionMethod = [RabbitMQ.Client.ConnectionFactory].GetMethod(“CreateConnection”, [Type]::EmptyTypes)
$connection = $createConnectionMethod.Invoke($factory, “instance,public”, $null, $null, $null)

Write-Host "Setting up RabbitMQ Model" -foregroundcolor green
$model = $connection.CreateModel()

Write-Host "Creating Exchange" -foregroundcolor green
$exchangeType = [RabbitMQ.Client.ExchangeType]::Fanout
$model.ExchangeDeclare("Module3.Sample11.Exchange", $exchangeType, $true)

Write-Host "Creating Worker Queue" -foregroundcolor green
$model.QueueDeclare(“Module3.Sample11.NormalQueue”, $true, $false, $false, $null)
$model.QueueBind("Module3.Sample11.NormalQueue", "Module3.Sample11.Exchange", "")

Write-Host "Creating Holding Queue" -foregroundcolor green
$args = @{"x-dead-letter-exchange"="Module3.Sample11.Exchange";};
$model.QueueDeclare(“Module3.Sample11.HoldingQueue”, $true, $false, $false, $args)




Write-Host "Setup complete"