sudo dnf install htop -y
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
chmod +x ./dotnet-install.sh
./dotnet-install.sh --version latest
PATH=$PATH:~/.dotnet
export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
dotnet --version

sudo echo -e "
[Unit]
Description=Some1 Wait

[Service]
WorkingDirectory=/home/ec2-user/publish/
ExecStart=/home/ec2-user/.dotnet/dotnet /home/ec2-user/publish/Some1.Wait.Back.MagicServer.dll
Restart=always
RestartSec=5
KillSignal=SIGINT
SyslogIdentifier=some1.wait
User=ec2-user
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
Environment=DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
Environment=COMPlus_DbgEnableMiniDump=1

[Install]
WantedBy=multi-user.target
" | sudo tee /etc/systemd/system/some1.wait.service

sudo systemctl enable some1.wait.service
sudo systemctl start some1.wait.service
systemctl status some1.wait.service
