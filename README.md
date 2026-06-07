# nScheduler

基于 **.NET 10** + **Blazor WebAssembly** + **BootstrapBlazor** 开发的容器化定时任务调度平台，支持 Docker 和 Kubernetes 两种运行环境，通过 Quartz.NET 实现高性能任务调度和执行。

---

## 📋 目录

- [功能特性](#-功能特性)
- [技术栈](#-技术栈)
- [项目结构](#-项目结构)
- [架构设计](#-架构设计)
- [快速开始](#-快速开始)
- [配置说明](#-配置说明)
- [API 接口](#-api-接口)
- [前端说明](#-前端说明)
- [部署指南](#-部署指南)

---

## ✨ 功能特性

- **定时任务管理**：支持创建、编辑、删除、启用/禁用定时任务
- **Cron 表达式**：灵活的 cron 表达式配置，支持复杂调度规则
- **容器化执行**：任务在 Docker 容器或 Kubernetes Pod 中隔离执行
- **多环境支持**：自动检测运行环境，无缝切换 Docker / Kubernetes
- **任务依赖**：支持配置任务执行依赖关系，形成任务链
- **参数配置**：支持环境变量和输入参数两种参数类型
- **任务日志**：实时记录任务执行状态和输出日志
- **消息通知**：任务执行完成后支持通过多种渠道发送通知（如机器人消息）
- **镜像管理**：统一管理任务使用的容器镜像
- **参数管理**：集中管理环境变量参数，方便复用
- **用户认证**：基于 JWT Bearer 的身份认证和权限管理
- **数据可视化**：通过 BootstrapBlazor 组件库提供友好的 Web UI

---

## 🛠 技术栈

| 层次 | 技术 |
|------|------|
| **运行时** | .NET 10.0 |
| **后端框架** | ASP.NET Core Web API |
| **前端框架** | Blazor WebAssembly |
| **UI 组件库** | BootstrapBlazor (AntDesign.X.Blazor) |
| **调度引擎** | Quartz.NET |
| **ORM** | Entity Framework Core |
| **数据库** | SQLite |
| **容器运行时** | Docker / Kubernetes |
| **认证** | JWT Bearer |
| **服务发现** | MediatR (CQRS 事件驱动) |

---

## 📁 项目结构

```
nScheduler/
├── nScheduler.sln                          # 解决方案文件
├── nScheduler.API/                         # API 层 (入口项目)
│   ├── Controllers/
│   │   ├── LoginController.cs              # 登录认证
│   │   ├── ImageCfgController.cs           # 镜像管理
│   │   ├── MessageCfgController.cs         # 消息通知配置
│   │   ├── ParameterCfgController.cs       # 参数配置管理
│   │   ├── UserController.cs               # 用户管理
│   │   ├── JobInfoController.cs            # 任务管理
│   │   └── JobLogController.cs             # 任务日志
│   ├── appsettings.json                    # 配置文件
│   └── quartz_tables.sql                   # Quartz 数据库初始化脚本
│
├── nScheduler.Domain/                      # 领域层 (DDD)
│   ├── Events/
│   │   └── ISchedulerEvent.cs              # 调度器事件接口
│   ├── Models/
│   │   ├── Configs/
│   │   │   ├── ImageCfg.cs                 # 镜像配置实体
│   │   │   ├── MessageCfg.cs               # 消息通知配置实体
│   │   │   ├── ParameterCfg.cs             # 参数配置实体
│   │   │   └── UserModel.cs                # 用户实体
│   │   └── Jobs/
│   │       ├── JobInfoModel.cs             # 任务定义实体
│   │       └── JobLogModel.cs              # 任务日志实体
│   ├── Repositories/
│   │   ├── IBaseRepository.cs              # 仓储接口
│   │   ├── Configs/                        # 配置仓储接口
│   │   └── Jobs/                           # 任务仓储接口
│   └── ViewModels/                         # 视图模型
│
├── nScheduler.Imp/                         # 实现层
│   ├── InitFactory.cs                      # 服务注册 & 种子数据
│   ├── Events/
│   │   ├── JobInfo/                        # 任务相关事件处理
│   │   │   ├── JobInfoPage.cs              # 任务分页查询
│   │   │   ├── JobInfoSingle.cs            # 任务详情查询
│   │   │   ├── JobInfoEdit.cs              # 任务编辑
│   │   │   ├── JobInfoDel.cs               # 任务删除
│   │   │   ├── JobInfoValid.cs             # 任务启用/禁用
│   │   │   └── JobInfoExec.cs              # 任务手动执行
│   │   ├── JobLog/                         # 日志相关事件处理
│   │   └── Msg/                            # 消息通知
│   │       ├── MsgSend.cs                  # 消息发送
│   │       └── RobotChatClient.cs          # 机器人客户端
│   ├── Jobs/
│   │   ├── SchedulerJob.cs                 # Quartz 定时任务实现
│   │   ├── SchedulerJobFactory.cs          # 任务工厂
│   │   └── QuartzOptions.cs                # Quartz 配置
│   └── Repositories/
│       ├── DataContext.cs                  # EF Core 上下文
│       ├── BaseRepository.cs               # 通用仓储实现
│       ├── Configs/                        # 配置仓储实现
│       └── Jobs/                           # 任务仓储实现
│
├── nScheduler.Exec/                        # 执行器层
│   ├── InitFactory.cs                      # 执行环境注册
│   ├── Dockers/
│   │   └── DockerEvent.cs                  # Docker 执行器
│   └── K8s/
│       └── KubernetesEvent.cs              # Kubernetes 执行器
│
├── nScheduler.Common/                      # 公共库
│   ├── Extensions/
│   │   ├── HttpRequest.cs                  # HTTP 请求封装
│   │   ├── ReflectHelper.cs                # 反射工具
│   │   └── StringExtension.cs              # 字符串工具
│   └── Models/
│       ├── BaseResult.cs                   # 统一响应模型
│       ├── ISearchModel.cs                 # 搜索接口
│       ├── JobStatus.cs                    # 任务状态枚举
│       ├── MessageType.cs                  # 消息类型枚举
│       └── ParameterType.cs                # 参数类型枚举
│
├── nScheduler.Web/                         # 前端 Blazor 项目
│   ├── Pages/
│   │   ├── Jobs/
│   │   │   ├── JobPage.razor               # 任务列表页
│   │   │   ├── JobCreate.razor             # 任务创建页
│   │   │   ├── JobEdit.razor               # 任务编辑页
│   │   │   ├── JobDetail.razor             # 任务详情页
│   │   │   └── JobLog.razor                # 任务日志页
│   │   ├── Configs/
│   │   │   ├── Image.razor                 # 镜像管理页
│   │   │   ├── ImageEditor.razor           # 镜像编辑器
│   │   │   ├── Message.razor               # 消息配置页
│   │   │   ├── Parameter.razor             # 参数配置页
│   │   │   └── User.razor                  # 用户管理页
│   │   └── Index.razor                     # 首页
│   ├── Accounts/
│   │   ├── AuthService.cs                  # 认证服务
│   │   ├── ApiAuthenticationStateProvider.cs
│   │   ├── Login.razor                     # 登录页
│   │   └── Logout.razor                    # 登出页
│   ├── Shared/
│   │   ├── CronSelector.razor              # Cron 表达式选择器
│   │   ├── DataGrid.razor                  # 数据网格组件
│   │   └── MainLayout.razor                # 主布局
│   └── Program.cs                          # 入口文件
│
└── nScheduler.Println/                     # 打印/导出服务
    ├── Dockerfile
    └── Program.cs
```

---

## 🏗 架构设计

### 分层架构

```
┌─────────────────────────────────────────────────────────┐
│                    nScheduler.API                        │
│                  (Controller Layer)                      │
│                    HTTP Request/Response                 │
└─────────────────────────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────┐
│                   nScheduler.Domain                      │
│              (Domain Models & Interfaces)                │
│   ┌─────────────┐  ┌──────────────┐  ┌─────────────┐   │
│   │   Entities   │  │   Enums      │  │  Interfaces  │   │
│   │  (DDD)       │  │              │  │ (Repositories│   │
│   └─────────────┘  └──────────────┘  │  / Events)   │   │
│   ┌─────────────┐                     └─────────────┘   │
│   │ ViewModels  │                                          │
│   └─────────────┘                                          │
└─────────────────────────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────┐
│                   nScheduler.Imp                         │
│              (Implementation Layer)                      │
│   ┌─────────────┐  ┌──────────────┐  ┌─────────────┐   │
│   │  Repositories│  │   Events     │  │   Quartz    │   │
│   │              │  │ (MediatR)    │  │   Jobs      │   │
│   └─────────────┘  └──────────────┘  └─────────────┘   │
└─────────────────────────────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────┐
│                   nScheduler.Exec                        │
│             (Execution Runtime)                          │
│   ┌──────────────┐    ┌──────────────┐                 │
│   │   Docker     │    │  Kubernetes  │                 │
│   │   Executor   │    │   Executor   │                 │
│   └──────────────┘    └──────────────┘                 │
└─────────────────────────────────────────────────────────┘
```

### 核心流程

```
定时触发 / 手动触发
       │
       ▼
┌─────────────┐
│  Quartz.NET  │  (调度引擎)
│   Scheduler  │
└──────┬──────┘
       │
       ▼
┌─────────────┐
│ SchedulerJob │  (任务执行器)
└──────┬──────┘
       │
       ▼
┌─────────────────────────────────┐
│     ISchedulerEvent (事件分发)   │
│  ┌──────────┐  ┌────────────┐  │
│  │ Docker   │  │ Kubernetes │  │
│  │ 执行器   │  │  执行器    │  │
│  └──────────┘  └────────────┘  │
└─────────────────────────────────┘
       │                    │
       ▼                    ▼
  创建容器              创建 Pod
       │                    │
       ▼                    ▼
  执行任务              执行任务
       │                    │
       ▼                    ▼
  记录日志              记录日志
       │
       ▼
   发送通知
```

---

## 🚀 快速开始

### 环境要求

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker](https://www.docker.com/) 或 [Kubernetes](https://kubernetes.io/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) 或 VS Code

### 本地运行

#### 1. 克隆项目

```bash
git clone <repository-url>
cd nScheduler
```

#### 2. 配置连接字符串

编辑 `nScheduler.API/appsettings.json`：

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=./bin/nscheduler.db"
  }
}
```

#### 3. 配置 JWT 认证

```json
{
  "JwtIssuer": "http://localhost:5000",
  "JwtAudience": "http://localhost:5265",
  "JwtSecurityKey": "YOUR_SECRET_KEY_CHANGE_IN_PRODUCTION"
}
```

#### 4. 启动 API 服务

```bash
cd nScheduler.API
dotnet restore
dotnet run
```

API 默认运行在 `http://localhost:5000`，Swagger UI 可通过 `https://localhost:5001/swagger` 访问。

#### 5. 启动前端

```bash
cd nScheduler.Web
dotnet restore
dotnet run
```

前端默认运行在 `http://localhost:5002`。

---

## ⚙️ 配置说明

### 执行环境配置

在 `appsettings.json` 中配置执行环境：

#### Docker 模式

```json
{
  "client": {
    "dockersock": "/var/run/docker.sock"
  }
}
```

#### Kubernetes 模式

```json
{
  "client": {
    "kubeconfig": "~/.kube/config",
    "namespace": "nScheduler-client"
  }
}
```

> **注意**：系统会自动检测运行环境。如果在集群内运行，自动使用 Kubernetes；否则优先使用 kubeconfig；最后回退到 Docker。

### Quartz 调度配置

```json
{
  "quartz": {
    "scheduler": {
      "instanceName": "MyScheduler",
      "instanceId": "AUTO"
    },
    "threadPool": {
      "type": "Quartz.Simpl.SimpleThreadPool, Quartz",
      "threadPriority": "Normal",
      "threadCount": 20
    },
    "jobStore": {
      "type": "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz",
      "tablePrefix": "QRTZ_",
      "driverDelegateType": "Quartz.Impl.AdoJobStore.StdAdoDelegate, Quartz",
      "clustered": "false"
    }
  }
}
```

---

## 🔌 API 接口

### 认证相关

| 方法 | 路径 | 说明 |
|------|------|------|
| POST | `/api/login` | 用户登录，返回 JWT Token |

### 任务管理

| 方法 | 路径 | 说明 |
|------|------|------|
| GET | `/api/jobinfo` | 获取任务列表（分页） |
| GET | `/api/jobinfo/{id}` | 获取任务详情 |
| POST | `/api/jobinfo` | 创建任务 |
| PUT | `/api/jobinfo/{id}` | 更新任务 |
| DELETE | `/api/jobinfo/{id}` | 删除任务 |
| POST | `/api/jobinfo/{id}/exec` | 手动执行任务 |

### 任务日志

| 方法 | 路径 | 说明 |
|------|------|------|
| GET | `/api/joblog` | 获取日志列表（分页） |
| GET | `/api/joblog/{id}` | 获取日志详情 |

### 配置管理

| 方法 | 路径 | 说明 |
|------|------|------|
| GET/POST/PUT/DELETE | `/api/imagecfg` | 镜像配置 CRUD |
| GET/POST/PUT/DELETE | `/api/messagecfg` | 消息通知配置 CRUD |
| GET/POST/PUT/DELETE | `/api/parametercfg` | 参数配置 CRUD |
| GET/POST/PUT/DELETE | `/api/user` | 用户管理 CRUD |

---

## 🖥 前端说明

### 功能模块

| 模块 | 路由 | 说明 |
|------|------|------|
| 任务列表 | `/job/page` | 查看、搜索、管理定时任务 |
| 任务创建 | `/job/create` | 新建定时任务 |
| 任务编辑 | `/job/edit/{id}` | 编辑现有任务 |
| 任务详情 | `/job/detail/{id}` | 查看任务详细信息 |
| 任务日志 | `/job/log` | 查看任务执行日志 |
| 镜像管理 | `/configs/image` | 管理容器镜像配置 |
| 消息配置 | `/configs/message` | 配置消息通知渠道 |
| 参数配置 | `/configs/parameter` | 管理任务参数 |
| 用户管理 | `/configs/user` | 管理系统用户 |

### 内置组件

- **[`CronSelector.razor`](nScheduler.Web/Shared/CronSelector.razor)**：Cron 表达式可视化选择器
- **[`DataGrid.razor`](nScheduler.Web/Shared/DataGrid.razor)**：封装的 DataGrid 组件

---

## 📦 部署指南

### Docker 部署

编辑 [`nScheduler.Println/Dockerfile`](nScheduler.Println/Dockerfile) 构建镜像：

```bash
docker build -t nscheduler:latest -f nScheduler.Println/Dockerfile .
docker run -d -p 5000:80 --name nscheduler nscheduler:latest
```

### Kubernetes 部署

```bash
# 创建命名空间
kubectl create namespace nScheduler-client

# 应用配置
kubectl apply -f deployment.yaml
kubectl apply -f service.yaml

# 查看状态
kubectl get pods -n nScheduler-client
kubectl logs -f deployment/nscheduler -n nScheduler-client
```

---

## 📝 默认账号

系统初始化时自动创建以下账号：

| 账号 | 密码 | 角色 |
|------|------|------|
| admin | 123456 | 管理员 |
| manager | 123456 | 操作员 |
| user | 123456 | 普通用户 |

---

## 📄 许可证

本项目仅供学习和研究使用。
