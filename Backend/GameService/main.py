from fastapi import FastAPI
from routers.websocket_routes import websocket_router
from routers.api_routes import api_router

app = FastAPI()

app.include_router(websocket_router)
app.include_router(api_router)