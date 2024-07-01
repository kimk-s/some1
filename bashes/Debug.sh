sudo dnf install lldb -y
dotnet tool install -g dotnet-symbol
dotnet tool install -g dotnet-dump
dotnet tool install -g dotnet-gcdump
dotnet tool install -g dotnet-sos
dotnet tool list -g
export PATH=$PATH:~/.dotnet/tools
export DOTNET_ROOT=~/.dotnet
dotnet-sos install



dotnet --list-runtimes
mkdir ~/dumps
/home/ec2-user/.dotnet/shared/Microsoft.NETCore.App/8.0.3/createdump 0000 -f ~/dumps/coredump.manual.%d



/tmp/coredump.0000



dotnet-symbol ~/dumps/ -o ~/dumps/symbols --host-only



lldb --core /tmp/coredump.4018
(lldb)setsymbolserver -directory ~/dumps/symbols
(lldb)clrstack
(lldb)clrthreads
(lldb)thread select 00
(lldb)bt
(lldb)pe
(lldb)dumpheap -stat -type System.InvalidOperationException
(lldb)dumpheap -mt ffff8643bb80
(lldb)dumpobj <address>