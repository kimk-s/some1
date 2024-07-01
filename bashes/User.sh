sudo dnf install htop -y
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
chmod +x ./dotnet-install.sh
./dotnet-install.sh --version latest
PATH=$PATH:~/.dotnet
export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
dotnet --version

sudo echo -e "
[Unit]
Description=Some1 User

[Service]
WorkingDirectory=/home/ec2-user/publish/
ExecStart=/home/ec2-user/.dotnet/dotnet /home/ec2-user/publish/Some1.User.CLI.dll run --address 0.0.0.0 --port 8000 --count 180 --time 1800 --fps 30
Restart=always
RestartSec=120
KillSignal=SIGINT
SyslogIdentifier=some1.user
User=ec2-user
Environment=DOTNET_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
Environment=DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
Environment=COMPlus_DbgEnableMiniDump=1

[Install]
WantedBy=multi-user.target
" | sudo tee /etc/systemd/system/some1.user.service

sudo systemctl enable some1.user.service
sudo systemctl start some1.user.service
systemctl status some1.user.service
