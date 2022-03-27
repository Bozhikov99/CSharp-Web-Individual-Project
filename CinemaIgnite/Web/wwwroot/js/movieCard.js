    let movieInfoElements=document.querySelectorAll('.card');
    movieInfoElements.forEach(mi=>mi.addEventListener('mouseover', ()=>{
        let infoElement=mi.querySelector('.movie-info');
        infoElement.removeAttribute('hidden');
    }));

    movieInfoElements.forEach(mi=>mi.addEventListener('mouseout', ()=>{
        let infoElement=mi.querySelector('.movie-info');
        infoElement.setAttribute('hidden', true);
    }));