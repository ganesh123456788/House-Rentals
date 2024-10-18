const formOpenBtn = document.querySelector("#form-open"),
home = document.querySelector(".home"),
formContainer = document.querySelector(".form_container"),
farBtn = document.querySelector(".far"),
signupBtn = document.querySelector("#signup"),
loginBtn = document.querySelector("#login"),
pwShowHide = document.querySelectorAll("#signup");

formOpenBtn.addEventListener("click", () => home.classList.add("show"))
farBtn.addEventListener("click", () => home.classList.remove("show"))
signupBtn.addEventListener("click", (e)=>{
    e.preventDefault()
    formContainer.classList.add("active")
})
loginBtn.addEventListener("click", (e)=>{
    e.preventDefault()
    formContainer.classList.remove("active")
})

const payment = document.querySelector(".pay")
function clickme(){
    // if(payment.style.display !='none'){
    //     payment.style.display ='block'
    // }else{
    //     payment.style.display = 'none'

    // }
    if(payment.style.display =='none'){
        payment.style.display = 'block'
    }else{

        payment.style.display ='none'
    }
}