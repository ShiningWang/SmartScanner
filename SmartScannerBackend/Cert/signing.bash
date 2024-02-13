openssl req -x509 -newkey rsa:4096 -sha256 -days 365 -nodes -keyout shiningdev.key -out shiningdev.crt -subj "/CN=shiningdev.com" -extensions v3_ca -extensions v3_req -config certificate.conf

openssl x509 -noout -text -in shiningdev.crt

openssl pkcs12 -export -out shiningdev.pfx -inkey shiningdev.key -in shiningdev.crt
# Export Password: shiningdev