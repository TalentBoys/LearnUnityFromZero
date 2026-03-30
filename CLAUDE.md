# CLAUDE.md - HelloWorld Unity 项目

## 项目概述
这是一个 **Unity 游戏开发教程项目**，用于从零学习 Unity 2D/3D 游戏开发，最终目标是上架 Steam。

## 教程大纲
📄 课程大纲文件：`Docs/curriculum.md`
- 共 4 个阶段、16 课
- 阶段1：Unity基础 + C#入门
- 阶段2：2D游戏实战
- 阶段3：3D游戏实战
- 阶段4：上架Steam
- 大纲中包含学习进度记录表，每完成一课请更新

## 环境信息
- **引擎**：团结引擎（Unity 中国版），3D (Core) 模板
- **项目路径**：
  - WSL: `/mnt/f/AIGame/HelloWorld/HelloWorld`
  - Windows: `F:\AIGame\HelloWorld\HelloWorld`
- **工作方式**：Claude Code 在 WSL 中编写代码文件，用户在 Windows 端 Unity 编辑器操作
- **代码语言**：C#

## 用户背景
- 有较熟练的编程基础
- 第一次做游戏开发
- 2D 和 3D 都要学

## 项目结构约定
```
Assets/
├── Scripts/          # C# 脚本
├── Scenes/           # 场景文件
├── Prefabs/          # 预制体
├── Materials/        # 材质
├── Textures/         # 贴图
├── Animations/       # 动画
├── Audio/            # 音效和音乐
└── UI/               # UI 资源
Docs/
└── curriculum.md     # 课程大纲与进度
```

## 教学原则
- 每节课先讲概念，再动手实践
- 代码中加必要注释帮助理解，但不过度注释
- 优先使用 Unity 内置功能，减少第三方依赖
- 每个阶段结束产出一个可运行的完整游戏
