# Веб сервис «Cowёr»

Онлайн-система для бронирования рабочих мест в коворкинге.

## Команда ТП №5:
-   [Павел Печенкин](https://github.com/IchinichiQ "Павел Печенкин") - Project-Manager, Backend-разработчик
-   [Тимофей Улезько](https://github.com/Lezko "Тимофей Улезько") - Frontend-разработчик, Дизайнер
-   [Елизавета Сидорова](https://github.com/lzaisd "Елизавета Сидорова") - Бизнес-аналитик, Тестировщик


### Использованные сервисы
- [YouTrack](https://cower.youtrack.cloud/agiles/) - Kanban-доска и база знаний
- [Figma](https://www.figma.com/file/lAdl4RMkU17MH4ga43bcP8/COWER?type=design) - Mockup-дизайн


### Документация
- Техническое задание
  [docx](documentation/Техническое_задание.docx)
  [pdf](documentation/Техническое_задание.pdf)
- Перечень задач по оформлению технического задания [pdf](documentation/Оформление_ТЗ.pdf)
- Сопроводительное письмо 
  [docx](documentation/Сопроводительное_письмо.docx)
  [pdf](documentation/Сопроводительное_письмо.pdf)
- Курсовой проект
  [docx](documentation/Курсовой_проект.docx)
  [pdf](documentation/Курсовой_проект.pdf)
- Программа и методика испытаний
  [docx](documentation/Программа_и_методика_испытаний.docx)
  [pdf](documentation/Программа_и_методика_испытаний.pdf)
- Протокол проведения испытаний
  [docx](documentation/Протокол_проведения_испытаний.docx)
  [pdf](documentation/Протокол_проведения_испытаний.pdf)
  
### Метрики
- [Скриншоты метрик](metrics)

### Приложение
- [Веб сайт](https://185.233.187.57/)
- [Swagger бэкенда](https://185.233.187.57:8081/swagger/index.html)

## Презентации проекта

[ТЗ видео (1 аттестация)](https://www.youtube.com/watch?v=h5_w42bOTcg)  
Презентация [pptx](documentation/Презентация.pptx) [pdf](documentation/Презентация.pdf)

[Видеопрезентация (2 аттестация)](https://youtu.be/67evfZUgtRQ)  
Презентация [pptx](documentation/Second.pptx) [pdf](documentation/Second.pdf)

Презентация к защите [pptx](documentation/Защита.pptx) [pdf](documentation/Защита.pdf)

[Видеопрезентация архитектуры бэкенда](https://youtu.be/VPsnFtl6sxA)  

[Видеопрезентация архитектуры фронтенда](https://youtu.be/kFqCJzGEbOM)  

[Видеопрезентация итогового проекта](https://www.youtube.com/watch?v=pWtdmeWd9Xg)  

[Видеогайд по использованию приложения](https://youtu.be/6ShIrcRhus4?si=YazHM6D5wZTgKBhe)  

## Запуск приложения
Приложениe использует docker.  
Для запуска бэкенда необходимо ввести следующую команду в папке `/app/backend`:
```
docker compose up -d
```  
Для запуска фронтенда аналогичную команду, но в папке `/app/frontend`.

Конфигурация приложения содержится в файлах `.env`, которые можно редактировать в текстовом редакторе. Для использования ssl необходимо:
- Для бэкенда добавить сертификат в формате `pfx` в папку `/app/backend/Cower.Web`, а также указать пароль от него в файле `/app/backend/.env`
- Для фронтенда добавить файлы `certificate.crt`, `private.key` и `ca_bundle.crt` в папку `/cower/ssl` на хост машине

### Данные учетной записи администратора по умолчанию
- **Email:** admin@cower.ru
- **Пароль:** `lyE2oSz5FhzxCQPsgZqs`

Настоятельно рекомендуем сменить пароль администратора при первом входе.