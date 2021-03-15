# Epitech Area
> *This is an Epitech 3rd-year project.*


## Introduction 👋

The goal of this project is to make a dashboard-style web app to gather feeds from multiple external APIs and centralize them into one interface.

The application is similar to [Netvibes](https://www.netvibes.com/fr)

## The project 🚀

**The project is organized in three parts:**
1. The mobile front-end is located under `mobile/`
2. The desktop/web front-end is located under `desktop/`
2. The back-end is located under `api/`

## Technologies used ⚙️
- Docker
- API:
    - ASP\.NET Core 3.1
    - Entity framework
- Desktop/web:
    - Electron
    - VueJS
    - VueX
    - TypeScript
- Mobile:
    - ReactNative
    - Expo
    - MobX
    - TypeScript

## Prerequisites

- Docker 🐳

## How to launch the project? 📲

1. Clone the project: `git clone https://github.com/Albert-Corson/area`

Then run the following commands to start the app.

### How to launch the API and web front-end? 🔮


```bash
docker-compose build && docker-compose up
```

### How to launch the Mobile app? 📲

**TODO**  
🚧 To be defined by [Louis Viot](https://github.com/lviot) 🚧  
cf: [issue #71](https://github.com/Albert-Corson/area/issues/71)

## Services and Widgets implemented

- Imgur 🌆
    - Imgur public gallery: See Imgur's most recent posts
    - Imgur favorites: See your favorite posts from Imgur
    - Imgur uploads: See your posts on Imgur
    - Imgur gallery search: Search for images on Imgur
- Spotify 🎧
    - Spotify favorite artists: Get a list of your favorite artists on Spotify
    - Spotify favorite tracks: Get a list of your favorite tracks
    - Spotify history: Get a list of your recently played tracks
- Microsoft 👨‍💼
    - Microsoft Calendar: View your calendar for next week
    - Microsoft Outlook unread mails: Get your most recent unread emails
    - Microsoft Todo list: View remaining tasks in your Todo list
- Lorem Picsum 🖼️
    - Lorem Picsum random Image: An inspiring image every time you load the widget
- NewsAPI 📰
    - Top headlines: See the most recent top headlines
    - News search: Search for headlines from the past month
- CatApi 🙀
    - Random cat images: See a bunch of cat images
- icanhazdadjoke 🥸
    - Random dad joke: Get a random dad joke


## Api documentation ? 📚

You will find documentation about the endpoints of the API at `[http://localhost:8080]/docs`.
Documentation about the different widgets can also be found in the [Wiki](https://github.com/Albert-Corson/area/wiki) section of this repository.

## Postman 🧪

You can import the collection under [/docs/api/swagger.json](https://github.com/Albert-Corson/area/blob/master/docs/api/swagger.json) to 
test the API with ease.

## Contributors

API (back-end):
- [Albert Corson](https://github.com/Albert-Corson)
- [Mathieu Pointecouteau](https://github.com/Krapaince)

Mobile app:  
- [Louis Viot](https://github.com/lviot)

Desktop/web app:  
- [Adrien Lucbert](https://github.com/adrienlucbert)
