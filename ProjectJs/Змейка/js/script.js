var canvas_game_shake = document.getElementById("game_shake");
var ctx = canvas_game_shake.getContext('2d');
let end=document.getElementById('end');
let text=document.getElementsByClassName("text");
let interval=-1;
function game(){
let num_win=20;
let speed_run=1;
let wall;
let mus_wall=[];
if(num_level==0){
    speed_run=2;
    num_win=10;
}
if(num_level==2){
    wall=new Image();
    wall.src="image/wall.jpg";
    for(let i=-1;i<10;i++){
        let el={
            X:canvas_game_shake.width/2,
            Y:30*i
        }
        mus_wall.push(el);
    }
}
let Keypress="";
document.body.addEventListener("keydown", function (e) {
    if((e.code=="KeyW"&&Keypress!="KewS")|| (e.code=="KeyS"&&Keypress!="KeyW")
        ||(e.code=="KeyA"&&Keypress!="KeyD")||(e.code=="KeyD"&&Keypress!="KeyA"))
    {
        Keypress=e.code;
    }
    else if(e.keyCode==38&&Keypress!="KewS"){
        Keypress="KeyW";
    }
    else if(e.keyCode==40&&Keypress!="KeyW"){
        Keypress="KeyS";
    }
    else if(e.keyCode==37&&Keypress!="KeyD"){
        Keypress="KeyA";
    }
    else if(e.keyCode==39&&Keypress!="KeyA"){
        Keypress="KeyD";
    }
    if(e.code=="Escape"){
        ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);
        clearInterval(interval);
        Keypress="";
        end.style.display="none";
        canvas_game_shake.style.display="none";
        menu.style.display="flex";
    }
})

var head=new Image();
head.src="image/head.jpg";
let head_width=head.style.width=30;
let head_height=head.style.height=30;
head.style.backgroundColor="black";
let head_object={
    X:Math.floor(Math.random()*(10))*head_width,
    Y:Math.floor(Math.random()*(10))*head_height
}
var strawberry= new Image();
let strawberry_width=strawberry.width=30;
let strawberry_height=strawberry.height=30;
strawberry.src="image/strawberry.png";
let strawberry_object={
    X:Math.floor(Math.random()*(10))*strawberry_width,
    Y:Math.floor(Math.random()*(10))*strawberry_height
}

let body=new Image();
body.width=head_width;
body.height=head_height;
body.src="image/body.png";
let mus_body=[];
function shake_if(){
    
    if(head_object.X==strawberry_object.X&&head_object.Y==strawberry_object.Y){
        strawberry_object.X=Math.floor(Math.random()*(10))*strawberry_width;
        strawberry_object.Y=Math.floor(Math.random()*(10))*strawberry_height;
        let isNul=false;
        if(mus_body.length==0){
            mus_body.push(head_object);
            isNul=true;
        }
        let length= mus_body.length-1;
        let body_object={
            X:0,
            Y:0
        }
        if( Keypress=="KeyW"){
            body_object.X=mus_body[length].X;
            body_object.Y=mus_body[length].Y+body.height;
        }
        else if( Keypress=="KeyS"){ 
            body_object.X=mus_body[length].X;
            body_object.Y=mus_body[length].Y-body.height;
        }
        else if( Keypress=="KeyA"){
            body_object.X=mus_body[length].X+body.width;
            body_object.Y=mus_body[length].Y;
        }
        else if(Keypress=="KeyD"){
            body_object.X=mus_body[length].X-body.width;
            body_object.Y=mus_body[length].Y
        }
        if(isNul){
            mus_body.pop();
            isNul=false;
        }
        mus_body.push(body_object);
    }
   
    
    if(mus_body.length!=0){
        let body_object={
            X:head_object.X,
            Y:head_object.Y,
        }
        mus_body.pop();
        mus_body.unshift(body_object);
        
    }
    
    if( Keypress=="KeyW"){
        head_object.Y-=head_height;
        head.src="image/head_top.jpg";
    }
    else if( Keypress=="KeyS"){
        head_object.Y+=head_height;
        head.src="image/head_bottom.jpg";
    }
    
    else if( Keypress=="KeyA"){
        head_object.X-=head_width;
        head.src="image/head_left.jpg";
    }
    else if( Keypress=="KeyD"){
        head_object.X+=head_width;
        head.src="image/head.jpg";
    }
    if(head_object.Y>=canvas_game_shake.height){
        head_object.Y=0;
    }
    if(head_object.Y<=-30){
        head_object.Y=canvas_game_shake.height;
    }
    if(head_object.X>=canvas_game_shake.width){
        head_object.X=0;
    }
    if(head_object.X<=-30){
        head_object.X=canvas_game_shake.width;
    }
   
}

function game_run(){
    
    ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);
    
    shake_if();
    for(let i=0;i<mus_body.length;i++){
        if(mus_body[i].X==strawberry_object.X&&mus_body[i].Y==strawberry_object.Y){
            setRand_object(strawberry_object,strawberry_width,strawberry_height)
        }
    }
    for(let i=0;i<mus_wall.length;i++){
        ctx.drawImage(wall,mus_wall[i].X,mus_wall[i].Y);
        if(mus_wall[i].X==strawberry_object.X&&mus_wall[i].Y==strawberry_object.Y){
            setRand_object(strawberry_object,strawberry_width,strawberry_height)
        }
    }
    ctx.drawImage(strawberry,strawberry_object.X,strawberry_object.Y);
    ctx.drawImage(head,head_object.X,head_object.Y);
    for(let i=0;i<mus_body.length;i++){
        ctx.drawImage(body,mus_body[i].X,mus_body[i].Y);
    }
    if(mus_body.length==num_win){
        ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);
        clearInterval(interval);
        canvas_game_shake.style.display="none";
        end.style.display="flex";
        text[0].textContent="Win";
        text[1].textContent="Total: "+mus_body.length;
    }
    for(let i=0;i<mus_body.length;i++){
        if(mus_body[i].X==head_object.X&&mus_body[i].Y==head_object.Y){
            ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);
            clearInterval(interval);
            canvas_game_shake.style.display="none";
            end.style.display="flex";
            text[0].textContent="Lose";
            text[1].textContent="Total: "+mus_body.length;
        }
    }
    for(let i=0;i<mus_wall.length;i++){
        if(mus_wall[i].X==head_object.X&&mus_wall[i].Y==head_object.Y){
            ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);
            clearInterval(interval);
            canvas_game_shake.style.display="none";
            end.style.display="flex";
            text[0].textContent="Lose";
            text[1].textContent="Total: "+mus_body.length;
        }
    }
}
function setRand_object(obj,width,height){
    if(obj!=undefined){
    obj.X=Math.floor(Math.random()*(10))*width;
    obj.Y=Math.floor(Math.random()*(10))*height;}
}
interval=setInterval(game_run,100*speed_run);
}
var menu=document.getElementById('menu');
var levels=document.getElementsByClassName('level');
let num_level=-1;
for(let i=0;i<levels.length;i++){
    levels[i].addEventListener('click',function(){
        menu.style.display='none';
        canvas_game_shake.style.display="flex";
        num_level=i;
        game();
    })
}
function go_menu(){
    end.style.display="none"
    menu.style.display="flex";
}
function restart(){
    end.style.display="none";
    canvas_game_shake.style.display="flex";
    game();
}