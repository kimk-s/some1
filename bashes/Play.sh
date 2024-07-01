sudo dnf install htop -y
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
chmod +x ./dotnet-install.sh
./dotnet-install.sh --version latest
PATH=$PATH:~/.dotnet
export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
dotnet --version

sudo echo -e "
[Unit]
Description=Some1 Play

[Service]
WorkingDirectory=/home/ec2-user/publish/
ExecStart=/home/ec2-user/.dotnet/dotnet /home/ec2-user/publish/Some1.Play.Server.Tcp.dll
Restart=always
RestartSec=5
KillSignal=SIGINT
SyslogIdentifier=some1.play
User=ec2-user
Environment=DOTNET_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
Environment=DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
Environment=COMPlus_DbgEnableMiniDump=1
Environment=Play__Id=-

[Install]
WantedBy=multi-user.target
" | sudo tee /etc/systemd/system/some1.play.service

sudo systemctl enable some1.play.service
sudo systemctl start some1.play.service
systemctl status some1.play.service
