var game_body=document.getElementById("MyCanvas");
var game_ctx=game_body.getContext('2d');
var menu=document.getElementsByClassName("menu")[0];
var end=document.getElementsByClassName("end")[0];
var end_res=document.getElementsByClassName("res")[0];
var total=document.getElementsByClassName("Total")[0];
let interval=-1;
var Keypress="";
let space;
let result=0;
let level;
function GameFlex(){
    end.style.display="none;"
    menu.style.display="none";
    game_body.style.display="flex";
}
function level1(){
    GameFlex();
    level=1;
    game();
    }
function level2(){
    GameFlex();
    level=2;
    game();
}
function level3(){
    GameFlex();
    level=3;
    game();
}
function game(){
    let Health_No=0;
let background=new Image();
background.src="image/space.jpg" 
background.style.width=960;
background.style.height=600;
game_ctx.drawImage(background,0,0,game_ctx.canvas.width,game_ctx.canvas.height)

document.body.addEventListener("keydown",function(e){
        if((e.code=="KeyW")|| (e.code=="KeyS")
        ||(e.code=="KeyA")||(e.code=="KeyD"))
        {
           Keypress=e.code;
        }
        else if(e.keyCode==38){
            Keypress="KeyW";
        }
        else if(e.keyCode==40){
            Keypress="KeyS";
        }
        else if(e.keyCode==37){
            Keypress="KeyA";
        }
        else if(e.keyCode==39){
            Keypress="KeyD";
        }  
        
    })   
let beat_go;
document.body.addEventListener("mousedown",function(event){
    if(event.button==0){
        beat_go=true;
    }
})
document.addEventListener("keydown",function(e){
    if(e.keyCode==32){
        space=true;
    }
})

document.body.addEventListener("mouseup",function(event){
    beat_go=false;
})

let beat=new Image();
let WidthBeat=beat.style.width=10; 
let HeightBeat= beat.style.height=20;
beat.src="image/2.png"
let MusBeat=[];

let beat_ult=new Image();
let WidthBeatUlt=beat_ult.style.width=20;
let HeightBeatUlt=beat_ult.style.height=game_ctx.canvas.height;
beat_ult.src="image/beatUlt.png"
let ObjectUlt={
    X:-1,
    Y:-1
}
var Fly1Enemy=new Image();
var WidthFly1Enemy=Fly1Enemy.style.width=40;
var HeightFly1Enemy= Fly1Enemy.style.height=40;
Fly1Enemy.src="image/Fly1enemy.png"
Fly1Enemy.style.position="absolute";
let MusEnemyFly=[];
let lenght_FLy1enemy=game_ctx.canvas.width/WidthFly1Enemy;
let step=20;
if(level==2){
for(let i=0;i<lenght_FLy1enemy;i++){
    let ObjectFly={
        X:0,
        Y:0,
        Health:5
    }
    let X=i*WidthFly1Enemy;
    let Y;
    if(i%2==1){
        Y=20
    }
    else {
        Y=HeightFly1Enemy+20;
    }
    ObjectFly.X=X;
    ObjectFly.Y=Y;
    MusEnemyFly.push(ObjectFly);
}}
else if(level==3){
    let length=lenght_FLy1enemy/3-1;
    for(let k=0;k<length;k++){
    for(let i=0;i<3;i++){
        for(let j=1;j<4;j++){
        let ObjectFly={
            X:0,
            Y:0,
            Health:5
        }
        let X=i*WidthFly1Enemy+k*WidthFly1Enemy+100*k;
        let Y=j*HeightFly1Enemy+20;

        ObjectFly.X=X;
        ObjectFly.Y=Y;
        MusEnemyFly.push(ObjectFly);}
    }}
}
else if(level==1){
    for(let k=0;k<7;k++){
        for(let i=0;i<3;i++){
            for(let j=0;j<3;j++){
            let ObjectFly={
                X:0,
                Y:0,
                Health:5
            }
            
            let X=i*WidthFly1Enemy+k*WidthFly1Enemy+100*k;
            let Y=j*HeightFly1Enemy+20;
            if(j==i){
                ObjectFly.X=X;
            ObjectFly.Y=Y;
            MusEnemyFly.push(ObjectFly);
            }
   
            
        }}
}}
var Fly1=new Image();
Fly1.style.width=WidthFly1Enemy;
Fly1.style.height=HeightFly1Enemy;
Fly1.src="image/Fly1.png"
let Fly1Object={
    X:game_ctx.canvas.width/2,
    Y:game_ctx.canvas.height-40,
    Health:20
}
let lenght_Fly1Object=Fly1Object.Health/MusEnemyFly[0].Health;
let Fly1EnemyFlee=false;
function ult_if(){
    if(space==true){
        ObjectUlt.X=Fly1Object.X+WidthBeatUlt/2;
        ObjectUlt.Y=Fly1Object.Y-HeightBeatUlt;
        countult=0;
        UltEnd=false;
    }
}
function Fire_If(){
    for(let i=0;i<MusBeat.length;i++){
        MusBeat[i].Y-=step;
    }
    if(beat_go==true &&Fly1Object.Y>=40&&UltEnd==true){
        let Object1={
            X:0,
            Y:0,
            Power:2.5
        }
        
        Object1.Y=Fly1Object.Y-10;

        Object1.X=Fly1Object.X+WidthBeat+WidthBeat/2;
        
        MusBeat.push(Object1);
    }
}
function Fly1enemy_if(){
    for(let i=0;i<MusEnemyFly.length;i++){
        MusEnemyFly[i].Y+=step;
    }
    
    for(let i=0;i<MusEnemyFly.length;i++){
        if(MusEnemyFly[i].Y<=game_ctx.canvas.height-10){
           return;
        }
    }
    Fly1EnemyFlee=true;
}
function Fly_if(){
    if(Keypress=="KeyW" &&Fly1Object.Y>=40){
      Fly1Object.Y-=step;  
    }
    if(Keypress=="KeyS"&& Fly1Object.Y<=game_ctx.canvas.height-50){
        Fly1Object.Y+=step;
    }
    if(Keypress=="KeyD"&&Fly1Object.X<=game_ctx.canvas.width-50){
        Fly1Object.X+=step;
    }
    if(Keypress=="KeyA"&&Fly1Object.X>=20){
        Fly1Object.X-=step;
    }
    
    
}
let countFly=0;
let countEnemyFly=0;
let countult=30;
var UltEnd=true;
function game_run(){
    game_ctx.clearRect(0, 0, game_ctx.canvas.width, game_ctx.canvas.height);
    countEnemyFly++;
    countFly++;
    if(countult<30){
        countult++;
    }
    if(countEnemyFly==20 ){
        
        countEnemyFly=0;    
        Fly1enemy_if();
    }
    
    if(countFly==2 && UltEnd==true){
        Fly_if();
        countFly=0;
    }
    if(countult==30){
        ult_if();

    }
    if(countult==15){
        ObjectUlt.X=-1;
        ObjectUlt.Y=-1;
        UltEnd=true;
        
        countFly=1;
    }
    if(countult<30){
        space=false;
    }
    Fire_If();
    let WidthConflict=20;
    let HeightConflict=20;
    for(let i=0;i<MusEnemyFly.length;i++){
        if((Fly1Object.X==MusEnemyFly[i].X||Fly1Object.X+WidthConflict==MusEnemyFly[i].X
            ||Fly1Object.X-WidthConflict==MusEnemyFly[i].X )&&
            (Fly1Object.Y==MusEnemyFly[i].Y||Fly1Object.Y+HeightConflict==MusEnemyFly[i].Y||
                Fly1Object.Y-HeightConflict==MusEnemyFly[i].Y)){
                let Health=MusEnemyFly[i].Health;
                MusEnemyFly[i].Health-=Fly1Object.Health;
                Fly1Object.Health-=Health;
                Health_No++;
            }
    }
    for(let i=0;i<MusEnemyFly.length;i++){
        for(let j=0;j<MusBeat.length;j++){
            for(let w=0;w<WidthFly1Enemy;w++){
                for(let h=0;h<HeightFly1Enemy;h++){
                    if(MusBeat[j].X==MusEnemyFly[i].X+w&&MusBeat[j].Y+h==MusEnemyFly[i].Y){
                        MusEnemyFly[i].Health-=2
                        MusBeat[j].Power=0
                    }
                }
            }
        }
    }
    if(ObjectUlt.X>0){
    for(let i=0;i<MusEnemyFly.length;i++){
        for(let w=0;w<WidthFly1Enemy;w++){
            if(MusEnemyFly[i].X+ w==ObjectUlt.X||
                MusEnemyFly[i].X-w==ObjectUlt.X){
                MusEnemyFly[i].Health-=10;
            }
        }
    }}
    for(let i=0;i<MusBeat.length;i++){
        if(MusBeat[i].Y<=40){
            MusBeat.splice(i,1);
        }
    }
    for(let i=0;i<MusBeat.length;i++){
        if(MusBeat[i].Power<=0){
            MusBeat.splice(i,1);
        }
    }
    for(let i=0;i<MusEnemyFly.length;i++){
        if(MusEnemyFly[i].Health<=0){
            MusEnemyFly.splice(i,1)
            result+=5;
        }
    }
    if(Fly1Object.Health<=0){
        game_lose();
    }
    if(Fly1EnemyFlee){
        game_win();
    }
    game_ctx.drawImage(background,0,0,game_ctx.canvas.width,game_ctx.canvas.height);
    
    
    for(let i=0;i<MusBeat.length;i++){
        game_ctx.drawImage(beat,MusBeat[i].X,MusBeat[i].Y,WidthBeat,HeightBeat)
    }
    if(ObjectUlt.X>0){
        game_ctx.drawImage(beat_ult,ObjectUlt.X,ObjectUlt.Y,WidthBeatUlt,HeightBeatUlt)
    }
    for(let i=0;i<MusEnemyFly.length;i++){
        game_ctx.drawImage(Fly1Enemy,MusEnemyFly[i].X,MusEnemyFly[i].Y,WidthFly1Enemy,HeightFly1Enemy)
    }
    game_ctx.drawImage(Fly1,Fly1Object.X,Fly1Object.Y,WidthFly1Enemy,HeightFly1Enemy)
    
    game_ctx.fillStyle="purple"
    game_ctx.font="18px serif"
    game_ctx.fillText("Score:"+result,20,20)
    if(countult==30){
        game_ctx.fillText("Ulta ready",100,20);
    }
    else {
        game_ctx.fillText("Ulta:"+countult,100,20)
    }
    game_ctx.fillStyle="DarkViolet"
    game_ctx.fillText("Starfighter",game_ctx.canvas.width/2-50,20)
    for(let i=0;i<lenght_Fly1Object;i++){
        if(Health_No>i){
            game_ctx.fillStyle="red"
        }
        else {
           game_ctx.fillStyle="green"
        }
        game_ctx.fillRect(game_ctx.canvas.width-60-(40*i),1,40,20)
    }
}
interval=setInterval(game_run,100);
}
function game_lose(){
    clearInterval(interval);
    game_body.style.display="none";
    Keypress="";
    end.style.display="flex";
    end_res.textContent="Lose"
    total.textContent="Total:"+result;

};
function game_win(){
    clearInterval(interval);
    game_body.style.display="none";
    Keypress="";
    end.style.display="flex";
    end_res.textContent="Win"
    total.textContent="Total:"+result;
}
document.addEventListener("keydown",function(e){
    if(e.key=="Escape"){
        ReturnMenu();
    }
})
function ReturnMenu(){
    game_body.style.display="none";
    menu.style.display="flex";
    end.style.display="none";
    clearInterval(interval);
    Keypress="";
}
function Return(){
    game_body.style.display="flex";
    menu.style.display="none";
    end.style.display="none";
    Keypress="";
    game();
}
