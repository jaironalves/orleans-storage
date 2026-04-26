#!/bin/bash
# Setup Guide para CarStateHandler com MySQL

echo "=== CarStateHandler Setup Guide ==="
echo ""

# 1. Criar banco de dados
echo "1. Criando banco de dados MySQL..."
mysql -h localhost -u root -p << EOF
CREATE DATABASE IF NOT EXISTS orleans-storage;
USE orleans-storage;

CREATE TABLE IF NOT EXISTS car (
  id INT PRIMARY KEY AUTO_INCREMENT,
  make VARCHAR(100),
  model VARCHAR(100),
  year INT,
  color VARCHAR(50),
  license_plate VARCHAR(20) UNIQUE NOT NULL,
  INDEX idx_license_plate (license_plate)
);

DESCRIBE car;
EOF

echo ""
echo "2. Verificar connection string em appsettings.json:"
echo '   "Silo-Mysql": "server=localhost;port=9006;database=orleans-storage;user=root;password=secret"'
echo ""

# 2. Compilar projeto
echo "3. Compilar solução..."
dotnet build

if [ $? -eq 0 ]; then
    echo "✅ Build successful!"
else
    echo "❌ Build failed!"
    exit 1
fi

echo ""
echo "4. Executar aplicação..."
dotnet run --project src/Orleans.Storage.API

echo ""
echo "=== Setup Completo ==="
