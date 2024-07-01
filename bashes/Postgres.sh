sudo dnf install htop -y
sudo dnf install postgresql15-server -y
sudo postgresql-setup --initdb
sudo systemctl enable postgresql
sudo systemctl start postgresql


sudo passwd postgres
su - postgres
psql -c "ALTER USER postgres WITH PASSWORD '';"
exit

# echo "postgres:" | sudo chpasswd
# sudo -i -u postgres psql -c "ALTER USER postgres WITH PASSWORD '';"


sudo cp /var/lib/pgsql/data/postgresql.conf /var/lib/pgsql/data/postgresql.conf.bck
sudo vi /var/lib/pgsql/data/postgresql.conf
# listen_addresses = '*' # what IP address(es) to listen on;


sudo cp /var/lib/pgsql/data/pg_hba.conf /var/lib/pgsql/data/pg_hba.conf.bck
sudo vi /var/lib/pgsql/data/pg_hba.conf
# You can change ident as md5 To allow connections from absolutely any address with password authentication
# host     all     all     0.0.0.0/0     md5
# sudo sed -i 's/ident$/md5/' /var/lib/pgsql/data/pg_hba.conf


sudo systemctl restart postgresql


# Connect to the PostgreSQL server as the Postgres user:
sudo -i -u postgres psql
# Create a new database user:
CREATE USER yourusername WITH PASSWORD '';
# Create a new database:
CREATE DATABASE database_name;
# Grant all privileges on the database to the user:
GRANT ALL PRIVILEGES ON DATABASE database_name TO yourusername;
# To list all available PostgreSQL users and databases:
\l


psql -h localhost -U username -d database_name
psql -h server-ip-address -U username -d database_name


sudo -i -u postgres
$ psql
postgres.
# To list the databases:
postgres=# \l
# You can exit Postgres prompt by typing:
postgres=# \q


sudo systemctl disable postgresql
sudo dnf remove postgresql15-server
sudo rm -rf /var/lib/pgsql /var/log/postgresql /etc/postgresql