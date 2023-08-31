# VersionControlSystem

Описание задания
> **Backup Object** – объект, который отслеживается системой для создания резервных копий. Объекты могут быть как файлами, так и папками.
> **Restore Point** – snapshot набора отслеживаемых объектов. Хранит как минимум дату создания и коллекцию **Backup Object**, отслеживаемых на момент создания **Restore Point**.
> **Backup** – в контексте данной системы, **Backup**, это коллекция **Restore Point**, то есть, полная история резервного копирования в рамках одной **Backup Task**.
> **Backup Task** – сущность хранящая конфигурацию резервного копирования (текущий список **Backup Object**, способов хранения, способов сжатия) и информацию о созданных **Restore Point**. Так же **Backup Task** должна инкапсулировать логику своего выполнения, т.е. создания новых **Restore Point**.
> **Repository** – абстракция над хранением и записью данных, будь то данные объектов, соответствующих **Backup Object**, либо данные о каком-либо **Storage**.
> **Storage** – файл, в котором хранится копия данных, соответствующих какому-либо **Backup Object**, созданная в конкретной **Restore Point**.

### Создание резервных копий
Под созданием резервной копии данных, подразумевается создание копии данных в другом месте. Система поддерживает расширяемость в вопросе выбора **Storage Algorithm**, используемых для хранения резервных копий. 
В данной работе реализуются два **Storage Algorithm**:
1. **Split Storage** – алгоритм раздельного хранения, для каждого **Backup Object** в **Restore Point** создаётся отдельный **Storage** - архив, в котором лежат данные объекта.
2. **Single Storage** – алгоритм общего хранения, для всех **Backup Object** в **Restore Point** создаётся один общий **Storage** - архив, в котором лежат данные каждого объекта.
**Storage Algorithm** не должен нести ответственность за реализацию архивации.

### Хранение копий
В работе подразумевается, что резервные копии будут создаваться локально на файловой системе. Но логика выполнения должна абстрагироваться от этого, поэтому вводится абстракция - репозиторий. 
Ожидаемая структура:
- Корневая директория
    - Директории различных **Backup Task**
        - Директории различных **Restore Point**
            - Файлы **Storage**

### Создание Restore Point
**Backup Task** отвечает за создание новых точек восстановления, выступает фасадом, инкапсулируя логику выполнения этой операции. 
При создании **Backup Task** должна быть возможность указать её название, **Repository** для хранения **Backup** (его данных), **Storage Algorithm**.
**Backup Task** должна поддерживать операции добавления и удаления отслеживаемых ей **Backup Object**.
Результатом выполнения **Backup Task** является создание **Restore Point** и соответствующих ей **Storage** в выбранном **Repository**.

# VersionControlSystem Extra
Расширенная версия работы.
К условиям из базовой версии добавляются следующие пункты:

### Алгоритмы очистки Restore Point
Помимо создания, нужно контролировать количество созданных **Restore Point**. Чтобы не допускать накопления большого количества старых и не актуальных **Restore Point**, реализуются механизмы их очистки, контролирующие соответствие набора существующих **Restore Point** заданному лимиту.
В рамках данной работы реализованы следующие виды лимитов:
1. По количеству **Restore Point** – храним коллекцию из последних **N** элементов, очищаем остальные.
2. По дате – ограничивает промежуток времени на котором хранятся **Restore Point**, элементы старее данного промежутка - очищаются.
3. Гибридные лимиты – пользователь может указывать как комбинировать лимиты: удалять **Restore Point** если он подходит под все требования/хотя бы под одно.

### Merge
Поддерживается альтернативное поведение при выходе за лимиты - **Merge**. 

**Merge** работает по правилам:
- Если в старой точке есть объект и в новой точке есть объект - нужно оставить новый, а старый можно удалять
- Если в старой точке есть объект, а в новой его нет - нужно перенести его в новую точку
- Если в точке объекты хранятся по правилу Single storage, то старая точка просто удаляется

### Logging
Логика работы системы не связана напрямую с консолью или другими внешними компонентами. Чтобы поддержать возможность уведомлять пользователя о событиях внутри алгоритма реализован вызов логирования. 

Задаваётся способ логирования извне.
В рамках данной работы реализованы следующие виды логирования:
- Консольный, который логирует информацию в консоль
- Файловый, который логирует в указанный файл

### Восстановление
Целью создания резервных копий является предоставление возможности восстановиться из них. 
Рреализован функционал, который позволяет указать **Restore Point** и восстановить данные из него. 

Поддерживается два режима восстановления:
- to original location - восстановить файл в то место, где находились отслеживаемый **Backup Object** (и заменить, если они ещё существуют)
- to different location - восстановить файл в указанный **Repository**