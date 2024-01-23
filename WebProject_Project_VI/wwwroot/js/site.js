// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// script.js

var likes = {};
var dislikes = {};
var views = {};

function like(id) {
    likes[id]++;
    updateLikes(id);
}

function dislike(id) {
    dislikes[id]++;
    updateDislikes(id);
}

function incrementViews(id) {
    views[id]++;
    updateViews(id);
}

function updateLikes(id) {
    document.getElementById('likes' + id).innerText = likes[id];
}

function updateDislikes(id) {
    document.getElementById('dislikes' + id).innerText = dislikes[id];
}

function updateViews(id) {
    document.getElementById('views' + id).innerText = views[id];
}

function toggleLikeDislike(id, action) {
    const likesBtn = document.getElementById('likeButton' + id);
    const dislikesBtn = document.getElementById('dislikeButton' + id);

    if (action === 'like') {
        if (!likes[id]) likes[id] = 0;
        likes[id]++;
        dislikes[id] = 0;
        updateLikes(id);
        updateDislikes(id);
    } else if (action === 'dislike') {
        if (!dislikes[id]) dislikes[id] = 0;
        dislikes[id]++;
        likes[id] = 0;
        updateDislikes(id);
        updateLikes(id);
    }
}

var btns = document.querySelectorAll('.likesbtn');

btns.forEach(function (btn) {
    btn.addEventListener('click', function () {
        // Toggle the appropriate class on the clicked button
        if (btn.classList.contains('green')) {
            btn.classList.remove('green');
            btn.classList.add('red');
        } else if (btn.classList.contains('red')) {
            btn.classList.remove('red');
        } else {
            btn.classList.add('green');
        }

        // Remove the opposite class from all buttons
        btns.forEach(function (otherBtn) {
            if (btn !== otherBtn && btn.classList.contains('green')) {
                otherBtn.classList.remove('red');
            } else if (btn !== otherBtn && btn.classList.contains('red')) {
                otherBtn.classList.remove('green');
            }
        });
    });
});

function toggleAccordion(id) {
    const accordionBody = document.getElementById(id);
    accordionBody.style.display = (accordionBody.style.display === 'none' || accordionBody.style.display === '') ? 'block' : 'none';
}
