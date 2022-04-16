# redisCache

# 1. Create Storage Class
> https://github.com/developer-onizuka/persistentVolume-CSI
```
$ kubectl get sc
NAME                   PROVISIONER      RECLAIMPOLICY   VOLUMEBINDINGMODE   ALLOWVOLUMEEXPANSION   AGE
nfs-vm-csi (default)   nfs.csi.k8s.io   Delete          Immediate           false                  17h
```

# 2. Deploying Redis on Kubernetes with Helm
If the pvc's status remains "Pending", the storageClass might be not "default".<br>
The annotation below should be used in metadata's section in the yaml of storageClass :<br>
```
  annotations:
    storageclass.kubernetes.io/is-default-class: "true"
```

See also the configuration parameters below:<br>
> https://github.com/bitnami/charts/tree/master/bitnami/redis
```
$ helm install redis bitnami/redis --set master.service.type=LoadBalancer

$ kubectl get pvc
NAME                          STATUS   VOLUME                                     CAPACITY   ACCESS MODES   STORAGECLASS   AGE
redis-data-redis-master-0     Bound    pvc-2eb9bb29-3e8f-4bb4-89ee-ac5d9f3728d2   8Gi        RWO            nfs-vm-csi     81m
redis-data-redis-replicas-0   Bound    pvc-62ed23c5-01c9-46a9-86c6-4caa51d26a12   8Gi        RWO            nfs-vm-csi     81m
redis-data-redis-replicas-1   Bound    pvc-849e6b27-9fdd-4ea7-b038-cedf5cb2eb88   8Gi        RWO            nfs-vm-csi     80m
redis-data-redis-replicas-2   Bound    pvc-219abaa1-1688-46a8-b626-01eb42f729b9   8Gi        RWO            nfs-vm-csi     80m
```

```
$ kubectl get pods
NAME                               READY   STATUS    RESTARTS        AGE
redis-master-0                     2/2     Running   0               75m
redis-replicas-0                   2/2     Running   0               75m
redis-replicas-1                   2/2     Running   0               74m
redis-replicas-2                   2/2     Running   0               74m

$ kubectl get services
NAME                  TYPE           CLUSTER-IP       EXTERNAL-IP      PORT(S)                                                         AGE
rabbitmq-headless     ClusterIP      None             <none>           4369/TCP,5672/TCP,25672/TCP,15672/TCP                           104m
redis-headless        ClusterIP      None             <none>           6379/TCP                                                        2m50s
redis-master          LoadBalancer   10.101.144.223   192.168.33.223   6379:32316/TCP                                                  2m50s
redis-replicas        ClusterIP      10.104.154.250   <none>           6379/TCP                                                        2m50s
```

# 3. PING command to test the connection with the server

The PONG response confirms that the server is listening.
```
$ REDIS_PASSWORD=$(kubectl get secret --namespace default redis -o jsonpath="{.data.redis-password}" | base64 --decode)

$ kubectl exec -it redis-master-0 -- redis-cli -a $REDIS_PASSWORD
Warning: Using a password with '-a' or '-u' option on the command line interface may not be safe.
127.0.0.1:6379> ping
PONG
127.0.0.1:6379> 
```

# 4. git clone and NuGet
```
$ https://github.com/developer-onizuka/redisCache
$ cd redis-test
$ dotnet add package Newtonsoft.Json
$ dotnet add package StackExchange.Redis
$ dotnet add package MongoDB.Driver
```
