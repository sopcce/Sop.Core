

```
cd /home/wwwroot/dotnet/readme.sopcce.com

dotnet Web.dll --server.urls http://localhost:5102 


47.93.18.104:5102 

# 


###

vi /usr/lib/systemd/system/readme.sopcce.com.service


#####
[Unit]
Description=readme.sopcce.com .NET Web API App running on CentOS

[Service]
WorkingDirectory=/home/wwwroot/dotnet/readme.sopcce.com
ExecStart=/usr/bin/dotnet /home/wwwroot/dotnet/readme.sopcce.com/Web.dll --server.urls http://localhost:5102 
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=dotnet-sop-readme-sopcce-com
User=root
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target



 ps -aux | grep readme.sopcce.com.service


 
systemctl daemon-reload

 

systemctl start readme.sopcce.com.service


systemctl stop readme.sopcce.com.service

systemctl restart readme.sopcce.com.service

systemctl status readme.sopcce.com.service

 journalctl -fu readme.sopcce.com.service

```

vi /usr/local/nginx/conf/nginx.conf


server {
    listen        80;
    server_name   example.com *.example.com;
    location / {
        proxy_pass         http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header   Upgrade $http_upgrade;
        proxy_set_header   Connection keep-alive;
        proxy_set_header   Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
    }
}






-- 备注S
systemctl restart nginx.service




-- 介绍文件

systemctl status nginx.service

